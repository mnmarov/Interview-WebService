using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
//using Zeus.Lib.Extensions;
using Zeus.Lib.WebServices.Controllers.Extensions;
using Zeus.Lib.WebServices.Controllers.Service;
using Zeus.Lib.WebServices.Interfaces;
using Zeus.Lib.WebServices.Models.Authorization;
using Zeus.Lib.WebServices.Models.ServiceSettings;

namespace Zeus.Lib.WebServices.Controllers
{
    public partial class ServiceTasks : IServiceTasks
    {
        //public ServiceBase[] ServicesToRun = null;
        public static WebServiceStatus ServiceStatus = new WebServiceStatus();
        private static System.Timers.Timer aTimer = null;

        public static WebServiceStatus Status
        {
            get { return ServiceStatus; }
        }

        public List<string> GetExcutableMethods()
        {
            var result = new List<string>();
            Type tp = GetType();
            var methods = tp.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (var m in methods)
            {
                result.Add(m.Name);
            }
            return result;
        }

        public static void LoadConfig()
        {
            var Users = WebService.Users;

            Status.ErrorCount = 0;
            Status.LastError = "";
            Status.LastErrorTime = DateTime.Now;
            /*
            int iIntervalMinutes = 0;
            string sIntervalMinutes = ExtensionHelpers.GetConfigString("IntervalMinutes");
            if (!int.TryParse(sIntervalMinutes, out iIntervalMinutes))
                iIntervalMinutes = 5;
            Status.IntervalMinutes = iIntervalMinutes;

            sIntervalMinutes = ExtensionHelpers.GetConfigString("IntervalMinutesLong");
            if (!int.TryParse(sIntervalMinutes, out iIntervalMinutes))
                iIntervalMinutes = 1440;

            ServiceLogger.LogFile = ExtensionHelpers.GetConfigString("LogFile");
            if (string.IsNullOrEmpty(ServiceLogger.LogFile))
                ServiceLogger.LogFile = "c:\\temp\\WebService.txt";

            // Setup log file for the rest of the application
            //OMSLib.Models.Helpers.SetLogFile(OMSService.OMSService.LogFile);

            Status.IntervalMinutesLong = iIntervalMinutes;

            //Status.TruncateTracking = Helpers.GetConfigBool("TruncateTracking");
            Status.MethodsToExecute = ExtensionHelpers.GetConfigString("MethodsToExecute");
            Status.MethodsToExecuteLong = ExtensionHelpers.GetConfigString("MethodsToExecuteLong");

            Status.Schedules.Clear();

            Users.Clear();

            try
            {
                var ss = (ServiceSettings)ConfigurationManager.GetSection("ServiceSettings");

                foreach (ServiceUserElement u in ss.Users)
                {
                    if (string.IsNullOrEmpty(u.Username))
                        continue;
                    if (string.IsNullOrEmpty(u.Password))
                        continue;
                    Users.Add(u.ToUser());
                }

                foreach (ServiceScheduleElement s in ss.Schedules)
                {
                    if (string.IsNullOrEmpty(s.MethodsToExecute))
                        continue;
                    var sch = s.ToSchedule();
                    sch.Normalize();
                    Status.Schedules.Add(sch);
                }
            }
            catch (Exception ex)
            {
                ServiceLogger.Error("Error reading configuration: " + ex.Message);
            }


            Status.NextTaskTime = DateTime.Now.AddMinutes(Status.IntervalMinutes);
            Status.NextLongTaskTime = DateTime.Now.AddMinutes(Status.IntervalMinutesLong);
            //*/
        }


        public object GetStatus()
        {
            return ServiceStatus;
        }

        public void StartTimer()
        {
            if (aTimer == null)
            {
                LoadConfig();

                aTimer = new System.Timers.Timer(5000);
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                aTimer.Enabled = true;
            }
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            aTimer.Enabled = false;

            Status.TickCount++;

            // Check for timeout
            WebServiceSession.Refresh();

            // Read Messages

            if ((DateTime.Now > Status.NextTaskTime))
            {
                Status.NextTaskTime = DateTime.Now.AddMinutes(Status.IntervalMinutes);
                // do timed stuff here
                ServiceLogger.Info("***** OnTimedEvent START short interval");
                DoTasks();
                ServiceLogger.Info("***** OnTimedEvent END short interval");
            }

            if ((DateTime.Now > Status.NextLongTaskTime))
            {
                //aTimer.Enabled = false;
                Status.NextLongTaskTime = DateTime.Now.AddMinutes(Status.IntervalMinutesLong);
                ServiceLogger.Info("***** OnTimedEvent START long interval");
                DoTasksLong();
                ServiceLogger.Info("***** OnTimedEvent END long interval");
                //aTimer.Enabled = true;
            }

            // Do Scheduled
            DoScheduled();

            aTimer.Enabled = true;
        }

        private static void DoScheduled()
        {
            foreach (var s in ServiceStatus.Schedules)
            {
                if (s.TimeOfDay < DateTime.Now)
                {
                    DoTasks(s.MethodsToExecute, s);
                    s.Advance();
                }
            }
        }

        public static void DoTasks()
        {
            DoTasks(Status.MethodsToExecute);
        }

        public static void DoTasksLong()
        {
            DoTasks(Status.MethodsToExecuteLong);
        }

        public static void DoTasks(string ExecuteMethods, ServiceSchedule schedule)
        {
            DoTasks(ExecuteMethods, new object[] { schedule });
        }

        public static void DoTasks(string ExecuteMethods, object[] parameters = null)
        {
            try
            {
                Status.TaskCount++;
                Status.TaskCalledTime = DateTime.Now;

                if (!string.IsNullOrEmpty(ExecuteMethods))
                {
                    Type TasksType = typeof(ServiceTasks);
                    var MethodNames = ExecuteMethods.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var Methods = TasksType.GetMethods();
                    var toExecute = Methods.Where(m => MethodNames.Contains(m.Name)).ToList();
                    foreach (var Method in toExecute)
                    {
                        try
                        {
                            // We only accept MethodName() and MethodName(ServiceSchedule schedule = null)
                            // You cannot call MethodName() from Schedule so make sure you have an overload
                            // WARNING: MethodName() is mandatory! The ServiceSchedule one is optional
                            int paraCount = Method.GetParameters().Length;
                            if (paraCount > 0)
                            {
                                if (parameters != null)
                                {
                                    Method.Invoke(null, parameters);
                                }
                            }
                            else if (parameters == null)
                            {
                                Method.Invoke(null, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            string message = ex.Message;
                            if (ex.InnerException != null)
                                message += " InnerException: " + ex.InnerException.Message;
                            ServiceLogger.Error("DoTasks invoke method error, Method: '" + Method.Name + "', details: " + message);
                        }
                    }
                }
                else
                {
                    //ServiceLogger.Error("DoTasks ExecuteMethods or ExecuteMethodsLong in .config is empty, please configure methods to execute!!!");
                }
            }
            catch (Exception ex)
            {
                ServiceLogger.Error("DoTasks Error, details: " + ex.Message);
            }
        }
    }
}

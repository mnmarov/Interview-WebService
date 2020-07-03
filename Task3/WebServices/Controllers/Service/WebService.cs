using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
//using Zeus.Lib.Crypto;
//using Zeus.Lib.Extensions.Models;
using Zeus.Lib.WebServices.Controllers;
using Zeus.Lib.WebServices.Interfaces;
using Zeus.Lib.WebServices.Models.Authorization;
using Zeus.Lib.WebServices.Models.Response;
using Zeus.Lib.WebServices.Models.ServiceSettings;

namespace Zeus.Lib.WebServices.Controllers.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class WebService : IWebService, IErrorHandler
    {
        public static List<WebServiceUser> Users = new List<WebServiceUser>();
        public static IServiceTasks Tasks = new ServiceTasks();

        #region Options
        public void AuthenticateOptions()
        {
        }
        public void SignOutOptions()
        {
        }
        public void StatusOptions()
        {
        }
        public void LogFileOptions()
        {
        }
        public void ExecuteTaskOptions()
        {
        }
        public void GetExcutableMethodsOptions()
        {
        }
        #endregion

        public AuthenticationResponse Authenticate(string username, string password)
        {
            WebServiceUser found = new WebServiceUser() { Username = "test", Password = "test" };
            AuthenticationResponse rsp = new AuthenticationResponse();
            if (found != null)
            {
                string SessionID = WebServiceSession.Start(found);

                WebOperationContext.Current.OutgoingResponse.Headers.Add(WebServiceAuthorization.SessionIDHeader + ": " + SessionID);
                WebOperationContext.Current.OutgoingResponse.Headers.Add(WebServiceAuthorization.UserIDHeader + ": " + found.ID);

                rsp.Status = (int)0;
                rsp.Token = SessionID;
                rsp.UserID = found.ID.ToString();
                rsp.Message = "Success";
            }
            else
            {
                rsp.Status = 404;
                rsp.Message = "Not authorized";
            }
            return rsp;
        }

        public bool SignOut()
        {
            return WebServiceSession.Terminate();
        }

        public WebServiceResponse GetStatus()
        {
            var result = new WebServiceResponse();
            result.Status = (int)0;// ErrorCodes.ALL_OK;
            result.Message = "Success";
            try
            {
                var Status = ServiceTasks.ServiceStatus;
                // This does not work know types should be set up before service object creation
                //WebServiceKnownTypesProvider.KnownTypes.Add(typeof(WebServiceStatus));
                result.Item = Status;
            }
            catch (Exception ex)
            {
                result.Status = (int)1;// ErrorCodes.GENERAL_ERROR;
                result.Message = ex.Message;
                ServiceLogger.Error("ExecuteTask error: " + ex.Message);
            }
            return result;
        }

        public string GetLogFileAsString()
        {
            return ServiceLogger.GetLogFile();
        }

        public WebServiceResponse ExecuteTask(string TaskNames)
        {
            if (!string.IsNullOrEmpty(TaskNames))
                TaskNames = TaskNames.Trim();
            var result = new WebServiceResponse();
            result.Status = (int)0;// ErrorCodes.ALL_OK;
            result.Message = "Success";
            try
            {
                ServiceTasks.DoTasks(TaskNames);
            }
            catch (Exception ex)
            {
                result.Status = (int)1;// ErrorCodes.GENERAL_ERROR;
                result.Message = ex.Message;
                ServiceLogger.Error("ExecuteTask error: " + ex.Message);
            }
            return result;
        }


        public WebServiceResponse GetExcutableMethods()
        {
            var result = new WebServiceResponse();
            result.Status = (int)0;// ErrorCodes.ALL_OK;
            result.Message = "Success";
            try
            {
                result.Items = Tasks.GetExcutableMethods().ToArray<object>();
            }
            catch (Exception ex)
            {
                result.Status = (int)1;// ErrorCodes.GENERAL_ERROR;
                result.Message = ex.Message;
                ServiceLogger.Error("ExecuteTask error: " + ex.Message);
            }
            return result;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            throw new NotImplementedException();
        }

        public bool HandleError(Exception error)
        {
            throw new NotImplementedException();
        }
    }
}

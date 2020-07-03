using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Zeus.Lib.WebServices.Controllers;
using Zeus.Lib.WebServices.Interfaces;

namespace Zeus.Lib.WebServices
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            if (Debugger.IsAttached)
            {
                var service = new SelfHostedService();
                service.OnDebug();

                string address = service.GetWebAddress();

                System.Diagnostics.Process.Start(address + "Login.html");

                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            }
            else
            {
                /*
                Tasks.ServicesToRun = new ServiceBase[]
                {
                new OMSServiceWindows()
                };
                ServiceBase.Run(Tasks.ServicesToRun);
                //*/
            }
        }
    }
}

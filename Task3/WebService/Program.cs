using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var service = new TestWebServiceWindows();
            service.OnDebug();
            Console.WriteLine("Service running.");
            string address = service.GetWebAddress();
            System.Diagnostics.Process.Start(address + "Login.html");
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
        }
    }
}

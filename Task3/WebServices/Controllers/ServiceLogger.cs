using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Zeus.Lib.WebServices.Controllers
{
    public class ServiceLogger
    {
        public static string LogFile = "c:\\temp\\WebService.txt";

        public static void Error(string Message)
        {
            var Status = ServiceTasks.ServiceStatus;
            Status.ErrorCount++;
            Status.LastError = Message;
            Status.LastErrorTime = DateTime.Now;

            //Zeus.Lib.Logger.Write(Message, LogFile, false, TraceEventType.Critical);
        }
        public static void Info(string Message)
        {
            //Zeus.Lib.Logger.Write(Message, LogFile);
        }

        public static string GetLogFile()
        {
            if (File.Exists(LogFile))
            {
                string[] lines = null;
                try
                {
                    lines = File.ReadAllLines(LogFile);
                }
                catch { }
                if (lines == null)
                    return "";
                Array.Reverse(lines);
                StringBuilder result = new StringBuilder();
                int cnt = 100;
                foreach (var line in lines)
                {
                    if (cnt <= 0)
                        break;
                    result.AppendLine(line);
                    cnt--;
                }
                return result.ToString();
            }
            return "Log file missing";
        }
    }
}

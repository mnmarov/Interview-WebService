using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Lib.WebServices.Models.ServiceSettings
{
    public class WebServiceStatus
    {
        public int TickCount = 0;
        public int TaskCount = 0;
        public int Count = 0;
        public DateTime TaskCalledTime = DateTime.Now;
        public DateTime NextTaskTime = DateTime.Now;
        public DateTime NextLongTaskTime = DateTime.Now;
        public int IntervalMinutes = 0;
        public int IntervalMinutesLong = 0;
        public int ErrorCount = 0;
        public string LastError = "";
        public DateTime LastErrorTime = DateTime.Now;
        public string MethodsToExecute = "";
        public string MethodsToExecuteLong = "";

        //public string OMSBaseURL = "";
        //public string OMSServiceBaseAddress = "";

        public DateTime StartedOn = DateTime.Now;

        public List<ServiceSchedule> Schedules = new List<ServiceSchedule>();
    }

}

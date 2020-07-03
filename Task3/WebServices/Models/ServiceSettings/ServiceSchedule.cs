using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Zeus.Lib.Extensions.Models;

namespace Zeus.Lib.WebServices.Models.ServiceSettings
{
    public class ServiceSchedule
    {
        //public ScheduleType Interval { get; set; }
        public string MethodsToExecute { get; set; }
        public DateTime TimeOfDay { get; set; }
        public int StartAt { get; set; }
        public string IntervalAdjustment { get; set; }
        public string Email { get; set; }
    }
}

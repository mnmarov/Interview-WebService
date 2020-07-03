using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Zeus.Lib.Extensions;
//using Zeus.Lib.Extensions.Models;
using Zeus.Lib.WebServices.Models.ServiceSettings;

namespace Zeus.Lib.WebServices.Controllers.Extensions
{
    public static class WebServiceExtensions
    {
        public static void Normalize(this ServiceSchedule result)
        {
            // Compute TimeOfDay (the date of first occurance) based on StartAt
            try
            {
                /*
                switch (result.Interval)
                {
                    case ScheduleType.WorkDays:
                        // If it's the weekend get monday
                        if (!result.TimeOfDay.IsWorkday())
                            result.Advance();
                        break;
                    case ScheduleType.Weekly:
                        result.TimeOfDay = result.TimeOfDay.SetDayOfWeek((DayOfWeek)result.StartAt);
                        break;
                    case ScheduleType.Monthly:
                        if (result.StartAt >= 31)
                        {
                            // End of month
                            result.TimeOfDay = result.TimeOfDay.SetDayOfMonth(1);

                            result.TimeOfDay = result.TimeOfDay.AddMonths(1);
                            result.TimeOfDay = result.TimeOfDay.AddDays(-1);
                        }
                        else if (result.StartAt > 0)
                            result.TimeOfDay = result.TimeOfDay.SetDayOfMonth(result.StartAt);
                        break;
                    default:
                        break;
                }
                while (result.TimeOfDay < DateTime.Now)
                    result.Advance();
                //*/
            }
            catch
            {
            }
        }

        public static void Advance(this ServiceSchedule self)
        {
            /*
            switch (self.Interval)
            {
                case ScheduleType.Weekly:
                    self.TimeOfDay = self.TimeOfDay.AddDays(7);
                    break;
                case ScheduleType.Monthly:
                    self.TimeOfDay = self.TimeOfDay.AddMonths(1);
                    break;
                case ScheduleType.WorkDays:
                    self.TimeOfDay = self.TimeOfDay.AddWorkdays(1);
                    break;
                //case ScheduleType.Daily:
                default:
                    self.TimeOfDay = self.TimeOfDay.AddDays(1);
                    break;
            }//*/
        }
    }

}

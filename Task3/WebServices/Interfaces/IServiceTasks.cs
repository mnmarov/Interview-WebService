using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Lib.WebServices.Interfaces
{
    public interface IServiceTasks
    {
        List<string> GetExcutableMethods();
        void StartTimer();
        //void DoTasks(string TaskNames, object[] parameters = null);
        object GetStatus();

        //object DoScheduled();

    }
}

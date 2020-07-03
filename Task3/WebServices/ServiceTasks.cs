using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeus.Lib.WebServices.Interfaces;

namespace Zeus.Lib.WebServices.Controllers
{
    public partial class ServiceTasks : IServiceTasks
    {
        public static void TestOperation()
        {
            ServiceLogger.Info("TestOperation called.");
        }
    }
}

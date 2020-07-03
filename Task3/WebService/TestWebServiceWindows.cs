using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
//using System.IdentityModel.Selectors;
using System.ServiceModel.Web;
using System.Configuration;
using Zeus.Lib;
using System.ServiceModel.Description;
using Zeus.Lib.WebServices.Controllers;
using Zeus.Lib.WebServices;
using WebService;

namespace WebService
{
    public partial class TestWebServiceWindows : SelfHostedService
    {
        public TestWebServiceWindows()
        {
            SelfHostedService.UseHTTPS = false;
            SelfHostedService.oServiceType = typeof(TestWebService);
            SelfHostedService.oServiceInterfaceType = typeof(ITestWebService);
            ServiceLogger.LogFile = "c:\\temp\\Service.txt";
            InitializeComponent();
        }
    }
}

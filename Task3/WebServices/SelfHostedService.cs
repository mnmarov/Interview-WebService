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
using System.ServiceModel.Web;
using System.ServiceModel.Description;
//using Zeus.Lib.Crypto;
using System.Collections.ObjectModel;
using System.Net.Security;
using System.Reflection;
using Zeus.Lib.WebServices.Interfaces;
using Zeus.Lib.WebServices.Controllers;
using Zeus.Lib.WebServices.Models;
//using Zeus.Lib.Extensions;
using Zeus.Lib.WebServices.Models.ServiceSettings;
using Zeus.Lib.WebServices.Controllers.Service;
using System.Configuration;
using Zeus.Lib.WebServices.Controllers.Extensions;

namespace Zeus.Lib.WebServices
{
    public partial class SelfHostedService : ServiceBase
    {
        public static bool UseHTTPS = true;

        internal ServiceHost oServiceHost = null;
        internal WebServiceHost oWebServiceHost = null;

        public static Type oServiceType = typeof(WebService);
        public static Type oServiceInterfaceType = typeof(IWebService);

        public static Type oServiceHTTPType = typeof(WebServiceHTTP);
        public static Type oServiceHTTPInterfaceType = typeof(IWebServiceHTTP);

        public static string DefaultHTTPSUrl = "https://localhost/TestWebService/";
        public static string DefaultHTTPUrl = "http://localhost/TestWebService/";

        public SelfHostedService()
        {
            InitializeComponent();
        }

        public string GetBaseAddress()
        {
            if (oServiceHost != null && oServiceHost.BaseAddresses.Count > 0)
                return oServiceHost.BaseAddresses[0].ToString();
            return "";
        }

        public string GetWebAddress()
        {
            if (oWebServiceHost != null && oWebServiceHost.BaseAddresses.Count > 0)
                return oWebServiceHost.BaseAddresses[0].ToString();
            return "";
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                //if (!SecureWebConfig.Secured)
                //{
                //    SecureWebConfig.Secure();
                //}
            }
            catch (Exception ex)
            {
                ServiceLogger.Error("Configuration cannot be read, reason: " + ex.Message);
            }


            ServiceTasks.LoadConfig();

            // collect and pass back the list of known types
            WebServiceKnownTypesProvider.KnownTypes.Add(typeof(WebServiceStatus));

            oServiceHost = new ServiceHost(oServiceType);

            try
            {
                if (oServiceHost.BaseAddresses.Count == 0)
                {
                    string ba = DefaultHTTPUrl; //ExtensionHelpers.GetConfigString("WebServiceBaseURL");
                    if (string.IsNullOrEmpty(ba))
                    {
                        if (UseHTTPS)
                            ba = DefaultHTTPSUrl;
                        else
                            ba = DefaultHTTPUrl;
                    }
                    else
                    {
                        if (ba.Contains("http://"))
                            UseHTTPS = false;
                    }

                    var baseAddresses = new Uri[] { new Uri(ba) };
                    oServiceHost = new ServiceHost(oServiceType, baseAddresses);

                    var endpoint = CreateEndpoint(oServiceInterfaceType, ba);
                    oServiceHost.AddServiceEndpoint(endpoint);

                    ServiceMetadataBehavior metadataBehavior = new ServiceMetadataBehavior();
                    //metadataBehavior.HttpGetEnabled = true;
                    oServiceHost.Description.Behaviors.Add(metadataBehavior);
                }

                oServiceHost.Authorization.ServiceAuthorizationManager = new RestAuthorizationManager();
                oServiceHost.Open();
            }
            catch (Exception ex)
            {
                ServiceLogger.Error("Service cannot be started, reason: " + ex.Message);
                return;
            }

            //Tasks.StartTimer();
            StartHTTPCompanionService(oServiceHost.BaseAddresses[0].ToString());

            ServiceLogger.Info("SERVICE STARTED");
            foreach (var uri in oServiceHost.BaseAddresses)
                ServiceLogger.Info("Listening on: " + uri);
        }

        public static ServiceEndpoint CreateEndpoint(Type stype, string url)
        {
            EndpointAddress address = new EndpointAddress(url);
            WebHttpBinding binding = new WebHttpBinding();
            WebHttpBehaviorWithErrors behavior = new WebHttpBehaviorWithErrors();
            if (UseHTTPS)
            {
                binding.Security.Mode = WebHttpSecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            }
            behavior.HelpEnabled = true;
            var contract = ContractDescription.GetContract(stype);
            var endpoint = new ServiceEndpoint(contract, binding, address);
            endpoint.Behaviors.Add(behavior);
            return endpoint;
        }

        private void StartHTTPCompanionService(string URL)
        {
            //URL = Zeus.Lib.Extensions.ExtensionHelpers.GetAddress(URL);
            WebServiceHTTP.WebServiceBaseAddress = URL;

            string WebServiceHTTPBaseAddress = URL + "Admin/";
            try
            {
                Uri uri = new Uri(WebServiceHTTPBaseAddress);
                oWebServiceHost = new WebServiceHost(oServiceHTTPType, uri);

                var endpoint = CreateEndpoint(oServiceHTTPInterfaceType, WebServiceHTTPBaseAddress);
                oWebServiceHost.AddServiceEndpoint(endpoint);

                //oWebServiceHost.Authorization.ServiceAuthorizationManager = new RestAuthorizationManager();
                oWebServiceHost.Open();
            }
            catch (Exception ex)
            {
                ServiceLogger.Error("Companion service cannot be started, reason: " + ex.Message);
                return;
            }

            ServiceLogger.Info("SERVICE HTML ADMIN STARTED");
            ServiceLogger.Info("Listening on: " + WebServiceHTTPBaseAddress);
        }

        protected override void OnStop()
        {
            if (oServiceHost != null)
            {
                oServiceHost.Close();
                oServiceHost = null;
            }
            if (oWebServiceHost != null)
            {
                oWebServiceHost.Close();
                oWebServiceHost = null;
            }
            ServiceLogger.Info("SERVICE STOPPED");
        }

        #region DESIGNER
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.ServiceName = "Self Hosted Service";
        }

        #endregion
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web.ApplicationServices;
using Zeus.Lib.WebServices.Controllers.Service;
using Zeus.Lib.WebServices.Models.Authorization;
using Zeus.Lib.WebServices.Models.Response;
using Zeus.Lib.WebServices.Models.ServiceSettings;

namespace Zeus.Lib.WebServices.Interfaces
{
    [ServiceContract]
    [ServiceKnownType("GetKnownTypes", typeof(WebServiceKnownTypesProvider))]
    public interface IWebService
    {
        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "/" + WebServiceAuthorization.AuthenticationMethod,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void AuthenticateOptions();

        //[OperationContract(IsInitiating = true)]
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/" + WebServiceAuthorization.AuthenticationMethod,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        AuthenticationResponse Authenticate(string username = "", string password = "");

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "/SignOut",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void SignOutOptions();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/SignOut",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool SignOut();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/Status",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        WebServiceResponse GetStatus();

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "/Status",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void StatusOptions();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/LogFile",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string GetLogFileAsString();

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "/LogFile",
        RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void LogFileOptions();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/ExecuteTask",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        WebServiceResponse ExecuteTask(string TaskName);

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "/ExecuteTask",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void ExecuteTaskOptions();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/GetExcutableMethods",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        WebServiceResponse GetExcutableMethods();

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "/GetExcutableMethods",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void GetExcutableMethodsOptions();
    }
}

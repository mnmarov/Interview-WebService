using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Zeus.Lib.WebServices.Models.Authorization;
using Zeus.Lib.WebServices.Models.Response;

namespace Zeus.Lib.WebServices.Interfaces
{
    [ServiceContract]
    public interface IZeusService
    {
        //[OperationContract(IsInitiating = true)]
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/" + WebServiceAuthorization.AuthenticationMethod,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        AuthenticationResponse Authenticate(string username = "", string password = "");

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "/" + WebServiceAuthorization.AuthenticationMethod,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        void AuthenticateOptions();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Zeus.Lib.WebServices.Models.Authorization;

namespace Zeus.Lib.WebServices.Controllers
{
    public class RestAuthorizationManager : ServiceAuthorizationManager
    {

        protected bool ReturnAllowAccess()
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin: *");
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Headers: Content-Type, Content-Length, User-ID, Session-ID");
            return true;
        }

        /// <summary>  
        /// </summary>  
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            // Only allow Authenticate if no session exists
            string method = WebServiceAuthorization.GetMethod();
            if (!string.IsNullOrWhiteSpace(method))
            {
                if (method.ToLower().Equals(WebServiceAuthorization.AuthenticationMethod.ToLower()))
                {
                    return ReturnAllowAccess();
                }
            }

            if (WebServiceAuthorization.isAuthorized(method))
            {
                return ReturnAllowAccess();
            }

            if (WebOperationContext.Current.IncomingRequest.Method.Equals("OPTIONS"))
            {
                return ReturnAllowAccess();
            }

            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;

            //return false;
            throw new WebFaultException(System.Net.HttpStatusCode.Unauthorized);
            //return true;
        }
    }
}

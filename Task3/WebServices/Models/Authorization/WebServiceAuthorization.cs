using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Lib.WebServices.Models.Authorization
{
    public class WebServiceAuthorization
    {
        public const string AuthenticationMethod = "Authenticate";
        public const string AuthenticationOptionsMethod = "AuthenticateOptions";
        public const string SessionIDHeader = "Session-ID";
        public const string UserIDHeader = "User-ID";

        public static bool isAuthorized(string method = null)
        {
            var session = WebServiceSession.Get();
            if (session != null)
            {
                string UserID = WebServiceSession.GetUserID();
                if (session.User.ID.ToString().Equals(UserID))
                {
                    // Check if we have permission for that method
                    if (!string.IsNullOrEmpty(method))
                    {
                        if (!string.IsNullOrEmpty(session.User.AllowedMethods))
                        {
                            string[] Methods = session.User.AllowedMethods.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            if (Methods != null)
                            {
                                if (!Methods.Contains(method))
                                {
                                    return false;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(session.User.ForbiddenMethods))
                        {
                            string[] Methods = session.User.ForbiddenMethods.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            if (Methods != null)
                            {
                                if (Methods.Contains(method))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    session.LastActivity = DateTime.Now;
                    return true;
                }
            }
            return false;
        }

        /*
        public static void CheckAuthorized()
        {
            if (!isAuthorized())
                throw new WebFaultException(HttpStatusCode.Unauthorized);
        }//*/

        public static string GetMethod()
        {
            string method = "";
            if (WebOperationContext.Current.IncomingRequest != null)
            {
                if (WebOperationContext.Current.IncomingRequest.UriTemplateMatch != null)
                {
                    method = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.Data as string;
                }
            }
            return method;
        }
    }
}

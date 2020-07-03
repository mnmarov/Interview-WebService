using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Lib.WebServices.Models.Authorization
{
    public class WebServiceUser
    {
        public Guid ID { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string AllowedMethods { get; set; }
        public string ForbiddenMethods { get; set; }
    }

    public class WebSession
    {
        public DateTime LastActivity { get; set; }
        public WebServiceUser User { get; set; }
        public Dictionary<string, object> Objects = new Dictionary<string, object>();
    }


    public class WebServiceSession
    {
        private static Dictionary<string, WebSession> m_Sessions = new Dictionary<string, WebSession>();

        private static int TimeoutMinutes = 5;
        /// <summary>
        /// Returns a session ID
        /// </summary>
        /// <returns>session ID</returns>
        public static string Start(WebServiceUser user)
        {
            string SessionID = Guid.NewGuid().ToString();
            m_Sessions[SessionID] = new WebSession() { User = user, LastActivity = DateTime.Now };
            return SessionID;
        }

        private static string GetParameter(string name)
        {
            var result = WebOperationContext.Current.IncomingRequest.Headers[name];
            if (string.IsNullOrEmpty(result))
            {
                if (WebOperationContext.Current.IncomingRequest.UriTemplateMatch != null)
                {
                    result = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters[name];
                }
            }
            return result;
        }

        private static string GetID()
        {
            var authHeader = GetParameter(WebServiceAuthorization.SessionIDHeader);
            if (string.IsNullOrEmpty(authHeader))
            {
                authHeader = "";
            }
            return authHeader;
        }

        public static string GetUserID()
        {
            var authHeader = GetParameter(WebServiceAuthorization.UserIDHeader);
            if (string.IsNullOrEmpty(authHeader))
                authHeader = "";
            return authHeader;
        }

        private static WebSession Get(string SessionID)
        {
            if (m_Sessions.ContainsKey(SessionID))
                return m_Sessions[SessionID];
            return null;
        }

        public static WebSession Get()
        {
            return Get(GetID());
        }

        public static bool Terminate()
        {
            string SessionID = GetID();
            if (string.IsNullOrEmpty(SessionID))
            {
                return false;
            }
            return m_Sessions.Remove(SessionID);
        }

        /// <summary>
        /// Session Timeout implementation
        /// </summary>
        public static void Refresh()
        {
            List<string> toRemove = new List<string>();
            foreach (var sessionID in m_Sessions.Keys)
            {
                var session = m_Sessions[sessionID];
                var difference = DateTime.Now - session.LastActivity;
                int dif = difference.Minutes;
                if (dif >= TimeoutMinutes)
                {
                    toRemove.Add(sessionID);
                }
            }
            foreach (var sessionID in toRemove)
            {
                m_Sessions.Remove(sessionID);
            }
        }
    }
}

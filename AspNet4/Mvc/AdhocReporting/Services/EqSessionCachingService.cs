using System;
using System.Web;
using System.Web.SessionState;

using Korzh.EasyQuery.Services;

namespace EqDemo.Services
{
    public class EqSessionCachingService : IEqCachingService
    {
        /// <summary>
        /// An instance of session object
        /// </summary>
        protected readonly HttpSessionState Session;

        public EqSessionCachingService()
        {
            Session = HttpContext.Current.Session;
        }

        public string GetValue(string key)
        {
            return (string)Session[key];
        }

        public void PutValue(string key, string value)
        {
            Session[key] = value;
        }
    }
}

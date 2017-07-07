using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Korzh.EasyQuery.AspNetCore.Demo03.Session
{
    /// <summary>
    /// Add for Session to .NET Core standart similar .NET Framework methods 
    /// </summary>
    public static class SessionExtentionMethods
    {
        /// <summary>
        /// Save to Session object with it's key
        /// </summary>
        public static void SetObject(this ISession session, string key, object value)
        {   
            string stringValue = JsonConvert.SerializeObject(value);
            session.SetString(key, stringValue);
        }

        /// <summary>
        /// Return from session Type object due to it's key
        /// </summary>
        public static T GetObject<T>(this ISession session, string key)
        {
            string stringValue = session.GetString(key);
            if (stringValue != null)
            {
                return JsonConvert.DeserializeObject<T>(stringValue);
            }
            return default(T);
        }


    }
}

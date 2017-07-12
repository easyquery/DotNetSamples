using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Korzh.EasyQuery.AspNetCore.Demo03.Session
{
    /// <summary>
    /// Adds for Session to .NET Core standart similar .NET Framework methods 
    /// </summary>
    public static class SessionExtentionMethods {
        /// <summary>
        /// Saves to Session object with it's key
        /// </summary>
        public static void SetObject(this ISession session, string key, object value) {
            string stringValue = JsonConvert.SerializeObject(value);
            session.SetString(key, stringValue);
        }

        /// <summary>
        /// Returns from session Type object by it's key
        /// </summary>
        public static T GetObject<T>(this ISession session, string key) {
            string stringValue = session.GetString(key);

            return (stringValue != null) ? JsonConvert.DeserializeObject<T>(stringValue) : default(T);
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpRequestExtensions
    {
        public static string GetLocalPath(this HttpRequest request)
        {
            if (!string.IsNullOrEmpty(request.PathBase)) {
                return  request.PathBase + request.Path;
            }
            else {
                return request.Path; 
            }
        }
    }
}

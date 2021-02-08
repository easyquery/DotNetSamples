using System.Web;
using System.Web.Mvc;

namespace EqDemo.AspNet4x.AdvancedSearch
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

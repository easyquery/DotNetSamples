using System;
using System.Configuration;
using System.Web.Http;
using System.Data.SqlClient;

using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.AspNet;

using EqDemo.Models;

namespace EqDemo.Controllers
{
    [RoutePrefix("api/easyquery")]
    public class AdvancedSearchController : EasyQueryApiController
    {
        protected override void ConfigureEasyQueryOptions(EasyQueryOptions options)
        {
            options.UseManager<EasyQueryManagerSql>();
            options.DefaultModelId = "nwind";
            options.BuildQueryOnSync = true;
            options.SaveQueryOnSync = false;

            options.UseDbContext(ApplicationDbContext.Create());

            // If you want to load model directly from DB metadata
            // remove (or comment) options.UseDbContext(...) call and uncomment the next 3 lines of code
            //options.ConnectionString = 
            //    ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ToString();
            //options.UseDbConnection<SqlConnection>();
            //options.UseDbConnectionModelLoader();

            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data");
            options.UseQueryStore((_) => new FileQueryStore(path));
        }
    }
}

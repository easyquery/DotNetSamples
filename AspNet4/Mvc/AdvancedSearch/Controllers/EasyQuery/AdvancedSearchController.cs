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

            // Uncomment this line if you want to load model directly from connection 
            // Do not forget to uncomment SqlClientGate registration in WebApiConfig.cs file
            //options.UseDbConnectionModelLoader();

            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data");
            options.UseQueryStore((_) => new FileQueryStore(path));
        }
    }
}

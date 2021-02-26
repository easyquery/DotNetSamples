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

            options.UseDbContext(ApplicationDbContext.Create());

            // Uncomment this line if you want to load model directly from connection 
            // Do not forget to uncomment SqlClientGate registration in WebApiConfig.cs file
            //options.UseDbConnectionModelLoader();

            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data");
            options.UseQueryStore((_) => new FileQueryStore(path));

            options.AddPreExecuteTuner(new SessionPreExecuteTuner());
        }
    }

    public class SessionPreExecuteTuner : IEasyQueryManagerTuner
    {
        public bool Tune(EasyQueryManager manager)
        {
            //An example of how you can add an extra condtion before query execution
            //var userId = (string)HttpContext.Current.Session["UserId"];
            //manager.Query.ExtraConditions.AddSimpleCondition("Users.Id", "Equal", userId);

            return true;
        }
    }
}
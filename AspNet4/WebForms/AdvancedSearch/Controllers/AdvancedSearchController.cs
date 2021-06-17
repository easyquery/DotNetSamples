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

            // Uncomment the following 3 lines if you want to load model directly from the DB's meta-data 
            // (it's better to remove UseDbContext(..) call abouve in this case)
            //options.ConnectionString = "Your connection string";
            //options.UseDbConnection<SqlConnection>();
            //options.UseDbConnectionModelLoader();

            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data");
            options.UseQueryStore((_) => new FileQueryStore(path));

            options.AddPreFetchTuner(manager => { 
                //any code you would like to execute before the data is retrieved from the DB
            });
        }
    }
}
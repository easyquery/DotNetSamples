using System;
using System.Web.Http;
using System.Configuration;

using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.AspNet;

using EqDemo.Models;
using EqDemo.Services;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace EqDemo.Controllers
{

    [RoutePrefix("api/adhoc-reporting")]
    [Authorize]
    public class EasyReportController : EasyQueryApiController
    {

        protected override void ConfigureEasyQueryOptions(EasyQueryOptions options)
        {
            //use EasyQuery manager that generates SQL queries
            options.UseManager<EasyQueryManagerSql>();

            options.DefaultModelId = "adhoc-reporting";

            options.StoreModelInCache = true;
            //it is required to register caching service, when StoreInCache is turned on
            options.UseCaching((_) => new EqSessionCachingService());

            //allow save query on sync for users with eq-manager role
            options.SaveQueryOnSync = User.IsInRole("eq-manager");
            options.SaveNewQuery = true;

            options.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ToString();
            options.UseDbConnectionModelLoader(settings => {
                settings.AddTableFilter(tbl => !tbl.Name.StartsWith("sys"));
                settings.AddFieldFilter(fld => !fld.IsForeignKey);
                settings.AddFieldFilter(fld => { 
                    if (fld.Name == "CompanyName") fld.Name = "Customer Name"; 
                    return true;
                }) ;
            });


            options.UseDbConnection<SqlConnection>();

            var dbContext = ApplicationDbContext.Create();

            options.UseQueryStore((_) => new ReportStore(dbContext, User));
        }
    }
}
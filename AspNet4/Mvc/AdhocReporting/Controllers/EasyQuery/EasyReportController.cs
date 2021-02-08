using System;
using System.Web.Http;

using Microsoft.AspNet.Identity.EntityFramework;

using Korzh.EasyQuery;
using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.AspNet;

using EqDemo.Models;
using EqDemo.Services;

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

            var dbContext = ApplicationDbContext.Create();
            options.UseDbContext(dbContext, config => { 

                // Ignore identity tables
                config.AddFilter((entityMap) => {

                    var entType = entityMap.Type;
                    return !(entType.IsInheritedFromGeneric(typeof(IdentityUser<,,,>))
                            || entType.IsInheritedFromGeneric(typeof(IdentityUserClaim<>))
                            || entType.IsInheritedFromGeneric(typeof(IdentityUserRole<>))
                            || entType.IsInheritedFromGeneric(typeof(IdentityUserLogin<>))
                            || entType.IsInheritedFromGeneric(typeof(IdentityRole<,>)));
                });

                // Ignore reports table
                config.AddFilter((entityMap) => {

                    var entType = entityMap.Type;
                    return entType != typeof(Report);
                });
            });

            options.UseQueryStore((_) => new ReportStore(dbContext, User));
        }
    }
}
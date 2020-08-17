using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;

using Korzh.DbUtils;

using Korzh.EasyQuery.DbGates;
using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.Db;

using EqAspNetCoreDemo.Services;
using EqAspNetCoreDemo.Models;


namespace EqAspNetCoreDemo
{
    public class Startup
    {

        private string _dataPath;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;

            this._dataPath = System.IO.Path.Combine(env.ContentRootPath, "App_Data");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EqDemoDb")));
            services.AddDefaultIdentity<IdentityUser>(opts => {
                //Password options
                opts.Password.RequiredLength = 4;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
             .AddRoles<IdentityRole>()
             .AddDefaultUI()
             .AddEntityFrameworkStores<AppDbContext>();

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddEasyQuery()
                    .UseSqlManager()
                    .AddDefaultExporters()
                    .RegisterDbGate<SqlClientGate>();

            // add default reports generatir
            services.AddScoped<DefaultReportGenerator>();

            services.AddMvc()
                    .AddNewtonsoftJson()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var appPathBase = Configuration["appPathBase"] ?? "/";
            app.UsePathBase(appPathBase);

            if (env.EnvironmentName == Environments.Development) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Disable all Identity/Account/Manage actions
            var redirectOptions = new RewriteOptions()
                .AddRedirect("(?i:identity/account/forgotpassword(/.*)?$)", "/")
                .AddRedirect("(?i:identity/account/manage(/.*)?$)", "/");
            app.UseRewriter(redirectOptions);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            
            //The middleware which handles the Advances Search scenario
            app.UseEasyQuery(options => {
                options.BuildQueryOnSync = true;
                options.DefaultModelId = "NWindSQL";
                options.ConnectionString = Configuration.GetConnectionString("EqDemoDb");
                options.UseDbConnection<SqlConnection>();

                //an example of using customer manager
                options.UseManager<CustomEasyQueryManagerSql>();

                //defining different query store depending on configuration
                if (Configuration.GetValue<string>("queryStore") == "session") {
                    options.UseQueryStore(services => new SessionQueryStore(services, "App_Data"));
                }
                else {
                    options.UseQueryStore(services => new FileQueryStore("App_Data"));
                }

                options.UseModelTuner(model => {
                    model.SortEntities();
                });

                //uncomment this line if you want to load model directly from connection 
                //options.UseDbConnectionModelLoader(loaderOptions => 
                //    loaderOptions.IgnoreViews()
                //                 //.DoNotReplaceToSingularNames()
                //                 .AddTableFilter(tableInfo => !(tableInfo.Name.StartsWith("Asp") || tableInfo.Name.StartsWith("__EF")))

                //);

                options.UseSqlFormats(FormatType.MsSqlServer, formats => {
                    formats.UseTimezoneOffset = true;
                });

                //The next line allows you to set SELECT DISTINCT for each generated query
                //options.AddBuilderTuner(builder => {
                //    (builder as SqlQueryBuilder).Extras.SelectDistinct = true;
                //});
                options.UsePaging(30);
            });

            app.UseEasyQuery(options => {
                options.DefaultModelId = "adhoc-reporting";
                options.SaveNewQuery = true;
                options.SaveQueryOnSync = true;
                options.Endpoint = "/api/adhoc-reporting";

                options.UseDbContextWithoutIdentity<AppDbContext>(loaderOptions => {
                    //Ignore the "Reports" DbSet as well
                    loaderOptions.AddFilter(entity => {
                        return entity.ClrType != typeof(Report);
                    });
                });

                // here we add our custom query store
                options.UseQueryStore((services) => new ReportStore(services));
                options.UsePaging(30);
                options.UseDefaultAuthProvider((provider) => {
                    //by default NewQuery, SaveQuery and RemoveQuery actions are accessible by the users with 'eq-manager' role 
                    //here you can remove that requirement and make those actions available for all authorized users
                    //provider.RequireAuthorization(EqAction.NewQuery, EqAction.SaveQuery, EqAction.RemoveQuery);

                    //here is an example how you can make some actions accessible only by users with a particular role.
                    //provider.RequireRole(DefaultEqAuthProvider.EqManagerRole, EqAction.NewQuery, EqAction.SaveQuery, EqAction.RemoveQuery);
                });

                options.AddPreExecuteTunningWithHttpContext((manager, context) => {
                    //the next two lines demonstrate how to add to each generated query a condition that filters data by the current user
                    //string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    //manager.Query.ExtraConditions.AddSimpleCondition("Employees.EmployeeID", "Equal", userId);
                });
            });

            //uncomment to test another approach for data filtering (available by /data-filtering2)
            //app.UseEasyQuery(options => {
            //    options.Endpoint = "/api/data-filtering2";
            //    options.UseEntity((services, _) => services.GetService<AppDbContext>()
            //                                               .Orders
            //                                               .Include(o => o.Customer)
            //                                               .Include(o => o.Employee)
            //                                               .AsQueryable());
            //    options.UsePaging(10);
            //});

            


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
            });


            //Init demo database (if necessary)
            app.EnsureDbInitialized(Configuration, env);
        }
    }
}

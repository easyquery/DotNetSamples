using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;

using Korzh.EasyQuery.DbGates;
using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.AspNetCore;

using EqAspNetCoreDemo.Services;

namespace EqAspNetCoreDemo
{
    public class Startup
    {

        private string _dataPath;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
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
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EqDemoDb")));
            services.AddDefaultIdentity<IdentityUser>(opts => {
                //Password options
                opts.Password.RequiredLength = 5;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
             .AddRoles<IdentityRole>()
             .AddDefaultUI(UIFramework.Bootstrap4)
             .AddEntityFrameworkStores<AppDbContext>();

            services.AddEasyQuery()
                    .UseSqlManager()
                    .AddDefaultExporters()
                    .RegisterDbGate<SqlClientGate>();

            // add default reports generatir
            services.AddScoped<DefaultReportGeneratorService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            //The middleware which handles the Advances Search scenario
            app.UseEasyQuery(options => {
                options.BuildQueryOnSync = true;
                options.DefaultModelId = "NWindSQL";
                options.ConnectionString = Configuration.GetConnectionString("EqDemoDb");
                options.UseDbConnection<SqlConnection>();
                //uncomment this line if you want to load model directly from connection 
                //options.UseDbConnectionModelLoader();
                options.UsePaging(30);
            });

            app.UseEasyQuery(options => {
                options.DefaultModelId = "adhoc-reporting";
                options.SaveQueryOnSync = true;
                options.Endpoint = "/api/adhoc-reporting";
                options.UseDbContext<AppDbContext>();
                options.UseDbConnection<SqlConnection>(Configuration.GetConnectionString("EqDemoDb"));

                // here we add our custom query store
                options.UseQueryStore((services) => new ReportStore(services));


                options.UseModelTuner((model) =>
                {
                    model.EntityRoot.Scan(ent => {
                        //Make invisible all entities started with "AspNetCore" and "Report"
                        if (ent.Name.StartsWith("Asp") || ent.Name == "Report")
                        {
                            ent.UseInConditions = false;
                            ent.UseInResult = false;
                            ent.UseInSorting = false;
                        }
                    }
                    , null, false);
                });

                options.UsePaging(30);
                options.UseDefaultAuthProvider((provider) =>
                {
                    //by default it is required eqmanager role 
                    provider.RequireAuthorization(EqAction.NewQuery, EqAction.SaveQuery, EqAction.RemoveQuery);
                    //provider.RequireRole(DefaultEqAuthProvider.EqManagerRole, EqAction.NewQuery, EqAction.SaveQuery, EqAction.RemoveQuery);
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

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //Init test database
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var dbContext = scopedServices.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }

            var scriptFilePath = System.IO.Path.Combine(_dataPath, "EqDemoDb.sql");
            var dbInit = new Data.DbInitializer(Configuration, "EqDemoDb", scriptFilePath);
            dbInit.AddTestData();
        }
    }
}

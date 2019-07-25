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

using Korzh.DbUtils;

using Korzh.EasyQuery.DbGates;
using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.AspNetCore;

using EqAspNetCoreDemo.Services;
using EqAspNetCoreDemo.Models;



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
                options.CheckConsentNeeded = context => false; // TO DO: true
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
             .AddDefaultUI(UIFramework.Bootstrap4)
             .AddEntityFrameworkStores<AppDbContext>();

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddEasyQuery()
                    .UseSqlManager()
                    .AddDefaultExporters()
                    .RegisterDbGate<SqlClientGate>();

            // add default reports generatir
            services.AddScoped<DefaultReportGenerator>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var appPathBase = Configuration["appPathBase"] ?? "/";
            app.UsePathBase(appPathBase);

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

            app.UseSession();
            app.UseAuthentication();

            //The middleware which handles the Advances Search scenario
            app.UseEasyQuery(options => {
                options.BuildQueryOnSync = true;
                options.DefaultModelId = "NWindSQL";
                options.ConnectionString = Configuration.GetConnectionString("EqDemoDb");
                options.UseDbConnection<SqlConnection>();
                //uncomment this line if you want to load model directly from connection 
                //options.UseDbConnectionModelLoader();

                if (Configuration.GetValue<string>("queryStore") == "session")
                {
                    options.UseQueryStore((services) => new SessionQueryStore(services));
                }
                else {

                    options.UseQueryStore((services) => new FileQueryStore("App_Data"));
                }
   
                options.UsePaging(30);
            });

            app.UseEasyQuery(options => {
                options.DefaultModelId = "adhoc-reporting";
                options.SaveQueryOnSync = true;
                options.Endpoint = "/api/adhoc-reporting";

                //Ignore identity classes
                options.UseDbContextWithoutIdentity<AppDbContext>(loaderOptions => {

                    //Ignore the "Reports" DbSet as well
                    loaderOptions.AddFilter(entity => entity.ClrType != typeof(Report));
                });

                options.UseDbConnection<SqlConnection>(Configuration.GetConnectionString("EqDemoDb"));

                // here we add our custom query store
                options.UseQueryStore((services) => new ReportStore(services));   

                options.UsePaging(30);
                options.UseDefaultAuthProvider((provider) =>
                {
                    //by default NewQuery, SaveQuery and RemoveQuery actions are accessible by the users with 'eqmanager' role 
                    //here you can remove that requirement and make those actions available for all authorized users
                    //provider.RequireAuthorization(EqAction.NewQuery, EqAction.SaveQuery, EqAction.RemoveQuery);

                    //here is an example how you can make some actions accessible only by users with a particular role.
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

            //Init demo database (if necessary)
            app.EnsureDbInitialized(Configuration, env);
        }
    }
}

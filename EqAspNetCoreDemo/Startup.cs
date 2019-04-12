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

using Korzh.EasyQuery.DbGates;
using Korzh.EasyQuery.Services;

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

            services.AddEasyQuery()
                    .UseSqlManager()
                    .RegisterDbGate<SqlClientGate>();

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

            app.UseEasyQuery(options => {
                options.BuildQueryOnSync = true;
                options.DefaultModelId = "NWindSQL";
                options.ConnectionString = Configuration.GetConnectionString("EqDemoDb");
                options.UseDbConnection<SqlConnection>();
                //uncomment this line if you want to load model directly from connection 
                //options.UseDbConnectionModelLoader(); uncomment this line
                options.UsePaging(10);
            });

            app.UseEasyQuery(options => {
                options.Endpoint = "/orders";
                options.UseEntity((services, _) => services.GetService<AppDbContext>()
                                                           .Orders
                                                           .Include(o => o.Customer)
                                                           .Include(o => o.Employee)
                                                           .AsQueryable());
                options.UsePaging(10);
            });

            app.UseEasyQuery(options => {
                options.SaveQueryOnSync = true;
                options.Endpoint = "/api-easyreport";
                options.UseDbContext<AppDbContext>();
                options.UseDbConnection<SqlConnection>(Configuration.GetConnectionString("EqDemoDb"));
                options.UseQueryStore((_) => new FileQueryStore(_dataPath));
                options.UsePaging(10);
            });


            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            var scriptFilePath = System.IO.Path.Combine(_dataPath, "EqDemoDb.sql");
            var dbInit = new Data.DbInitializer(Configuration, "EqDemoDb", scriptFilePath);
            dbInit.EnsureCreated();
        }
    }
}

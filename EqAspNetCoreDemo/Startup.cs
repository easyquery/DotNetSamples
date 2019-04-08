﻿using System;
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
                options.DefaultModelId = "NWindSQL";
                options.ConnectionString = Configuration.GetConnectionString("EqDemoDb");
                options.UseDbConnection<SqlConnection>();
                options.UseDbConnectionModelLoader();
                options.UsePaging(10);
            });

            app.UseEasyQuery(options => {
                options.Endpoint = "/orders";
                options.UseEntity((services, _) => services.GetService<AppDbContext>()
                                                           .Orders
                                                           .Include(o => o.Customer)
                                                           .Include(o => o.Employee)
                                                           .AsQueryable());
                options.UsePaging(20);
            });

            app.UseEasyQuery(options => {
                options.Endpoint = "/easyreport";
                options.UseDbContext<AppDbContext>();
                options.UseDbConnection<SqlConnection>();
                options.UsePaging(10);
            });


            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope()) {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var dbInit = new DbInitializer(dbContext, _dataPath);
                dbInit.CheckDb();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Korzh.WindowsAzure.Storage;

using Korzh.EasyQuery.AspNetCore;

using EqAzureDemo.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EqAzureDemo {
    public class Startup {

        private string _dataPath;

        public Startup(IConfiguration configuration, IHostingEnvironment env) {
            Configuration = configuration;
            this._dataPath = System.IO.Path.Combine(env.ContentRootPath, "App_Data");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            services.AddAzureStorageContext<AzureStorageContext>(
                options=> options.ConnectionString = Configuration.GetConnectionString("AzureStorage")
                );

            services.AddTransient<NwindContext>();

            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc()
                  .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddEasyQuery();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope()) {
                var dbContext = scope.ServiceProvider.GetRequiredService<NwindContext>();
                var dbInit = new DbInitializer(dbContext, _dataPath);
                dbInit.CheckDb();
            }
        }
    }
}

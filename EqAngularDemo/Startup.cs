using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using Korzh.EasyQuery.AspNetCore;

using EqAngularDemo.Data;

namespace EqAngularDemo {
    public class Startup {

        private string _dataPath;

        public Startup(IHostingEnvironment env, IConfiguration configuration) {
            Configuration = configuration;
            this._dataPath = System.IO.Path.Combine(env.ContentRootPath, "App_Data");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            var connectionString = Configuration.GetConnectionString("EqAngularDemoDb");

            services.AddDbContext<AppDbContext>(opts => {
                opts.UseSqlServer(connectionString);
            });

            services.AddMvc();

            services.AddEasyQuery();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true
                });
            }
            else {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope()) {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var dbInit = new DbInitializer(dbContext, _dataPath);
                dbInit.CheckDb();
            }
        }
    }
}

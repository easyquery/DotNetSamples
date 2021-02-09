using System.Data.SqlClient;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

using Korzh.EasyQuery.Services;

namespace EqDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("EqDemoDb"));
            });

            services.AddControllersWithViews();

            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowAllPolicy",
                    builder => {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                        builder.WithExposedHeaders("Content-Disposition");
                    });
            });

            services.AddEasyQuery()
                 .AddDefaultExporters()
                 .UseSqlManager();
            // Uncomment if you want to load model directly from DB               
            // .RegisterDbGate<SqlServerGate>();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/www";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseCors("AllowAllPolicy");

            app.UseRouting();

            app.UseEasyQuery(options => {
                options.DefaultModelId = "nwind";

                options.SaveNewQuery = false;

                options.UseDbContext<AppDbContext>();

                // Uncomment if you want to donwload model directly from DB
                // options.UseDbConnectionModelLoader();

                options.UseQueryStore((_) => new FileQueryStore("App_Data"));
            });

            app.UseEasyQuery(options => {
                options.Endpoint = "/api/data-filtering";

                options.DefaultModelId = "nwind";
                options.UseDbContext<AppDbContext>();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment()) {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4444/");
                }
            });

            //Init demo database (if necessary)
            app.EnsureDbInitialized(Configuration, env);
        }
    }
}

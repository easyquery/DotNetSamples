using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

using VueCliMiddleware;

using Korzh.EasyQuery.Services;
using EasyData.Export;
using Korzh.EasyQuery.Db;

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
                options.UseSqlite(Configuration.GetConnectionString("EqDemoSqLite"));
                //options.UseSqlServer(Configuration.GetConnectionString("EqDemoDb"));
            });

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

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddEasyQuery()
                    .UseSqlManager()
                    .AddDefaultExporters()
                    .AddDataExporter<PdfDataExporter>("pdf")
                    .AddDataExporter<ExcelDataExporter>("excel");
                    // Uncomment if you want to load the model directly from DB metadata              
                    // .RegisterDbGate<SqLiteGate>();
                    // .RegisterDbGate<SqlServerGate>();
                    
            //to support non-Unicode code pages in PDF Exporter
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors("AllowAllPolicy");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment()) {
                app.UseSpaStaticFiles();
            }

       
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapEasyQuery(options => {
                    options.DefaultModelId = "nwind";

                    options.SaveNewQuery = false;

                    options.UseDbContext<AppDbContext>();

                    // Uncomment if you want to download model directly from DB
                    // options.UseDbConnectionModelLoader();

                    options.UseQueryStore((_) => new FileQueryStore("App_Data"));

                    options.UseSqlFormats(formats => {
                        formats.OrderByStyle = OrderByStyles.Names;
                    });
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            //app.UseSpa(spa => {

            //    spa.Options.SourcePath = "ClientAppNew";
            //    spa.Options.StartupTimeout = TimeSpan.FromMinutes(2);

            //    if (env.IsDevelopment()) {
            //        // run npm process with client app
            //        spa.UseVueCli(npmScript: "dev", port: 8086, regex: "vite");
            //        // if you just prefer to proxy requests from client app, use proxy to SPA dev server instead:
            //        // app should be already running before starting a .NET client
            //        spa.UseProxyToSpaDevelopmentServer("http://localhost:8086"); // your Vue app port
            //    }
            //});


            //Init demo database (if necessary)
            app.EnsureDbInitialized(Configuration, env);
        }
    }
}

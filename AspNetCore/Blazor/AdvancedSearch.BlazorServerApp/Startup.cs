using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;

using EasyData.Export;

using Korzh.EasyQuery.Services;

using EqDemo.Data;
using EqDemo.Services;

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
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(
              options => options.UseSqlServer(Configuration.GetConnectionString("EqDemoDb")));

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddEasyQuery()
                    .UseSqlManager()
                    .AddDefaultExporters()
                    .AddDataExporter<PdfDataExporter>("pdf")
                    .AddDataExporter<ExcelDataExporter>("excel")
                    .RegisterDbGate<Korzh.EasyQuery.DbGates.SqlServerGate>();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
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

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapEasyQuery(options => {
                    options.DefaultModelId = "nwind";
                    options.SaveNewQuery = false;
                    options.BuildQueryOnSync = true;
                    options.UseDbContext<AppDbContext>();

                    // If you want to load model directly from DB metadata
                    // remove (or comment) options.UseDbContext(...) call and uncomment the next 3 lines of code
                    //options.ConnectionString = Configuration.GetConnectionString("EqDemoDb");
                    //options.UseDbConnection<Microsoft.Data.SqlClient.SqlConnection>();
                    //options.UseDbConnectionModelLoader();

                    options.UseQueryStore((_) => new FileQueryStore("App_Data"));
                });

                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            //Init demo database (if necessary)
            app.EnsureDbInitialized(Configuration, env);
        }
    }
}

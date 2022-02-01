using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using EasyData;
using EasyData.Export;
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
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => {
                configuration.RootPath = "ClientApp/dist";
            });
			
			//Registering EasyQuery services
			services.AddEasyQuery()
                .AddDefaultExporters()
                .AddDataExporter<PdfDataExporter>("pdf")
                .AddDataExporter<ExcelDataExporter>("excel")
                .UseSqlManager();
			// Uncomment if you want to load model directly from DB               
			// .RegisterDbGate<SqlServerGate>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment()) {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
					
				//Adding EasyQuery API middleware
				endpoints.MapEasyQuery(options => {
				    options.DefaultModelId = "nwind";

				    options.SaveNewQuery = false;
				    options.BuildQueryOnSync = true;

				    options.UseDbContext<AppDbContext>();

				    // Uncomment if you want to donwload model directly from DB
				    // options.UseDbConnectionModelLoader();

				    options.UseModelTuner(manager =>
				    {
				        // for onGetExpression example
				        var attr = manager.Model.FindEntityAttr("Customer.CompanyName");
				        if (attr != null) {
				            attr.DefaultEditor = new TextValueEditor("ContactNameEditor");
				        }
				    });

				    options.UseQueryStore((_) => new FileQueryStore("App_Data"));
				});
            });

            app.UseSpa(spa => {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment()) {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using EasyData;
using EasyData.Export;

using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.Db;

using EqDemo.Services;
using Microsoft.Data.Sqlite;

namespace EqDemo
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Korzh.EasyQuery.RazorUI.Pages.AdvancedSearch.ExportFormats = new string[] { "pdf", "excel", "excel-html", "csv" };
            
            //uncomment the following line if you want to show the SQL statements on each change in your query
            //Korzh.EasyQuery.RazorUI.Pages.AdvancedSearch.ShowSqlPanel = true;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {          
            services.AddDbContext<AppDbContext>(
                options => options.UseSqlite(Configuration.GetConnectionString("EqDemoSqLite"))
                //options => options.UseSqlServer(Configuration.GetConnectionString("EqDemoDb"));
            );
       
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddEasyQuery()
                    .UseSqlManager()
                    .AddDefaultExporters()
                    .AddDataExporter<PdfDataExporter>("pdf")
                    .AddDataExporter<ExcelDataExporter>("excel")
                    .RegisterDbGate<Korzh.EasyQuery.DbGates.SqLiteGate>();
                    //.RegisterDbGate<Korzh.EasyQuery.DbGates.SqlServerGate>();

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapEasyQuery(options => {
                    options.DefaultModelId = "nwind";
                    options.BuildQueryOnSync = true;
                    options.SaveNewQuery = false;
                    options.ConnectionString = Configuration.GetConnectionString("EqDemoSqLite");
                    options.UseDbConnection<SqliteConnection>();
                    options.UseDbContext<AppDbContext>();

                    // If you want to load model directly from DB metadata
                    // remove (or comment) options.UseDbContext(...) call and uncomment the next 3 lines of code
                    //options.ConnectionString = Configuration.GetConnectionString("EqDemoDb");
                    //options.UseDbConnection<Microsoft.Data.SqlClient.SqlConnection>();
                    //options.UseDbConnectionModelLoader();

                    options.UseQueryStore((_) => new FileQueryStore("App_Data"));

                    options.UseModelTuner(manager => {
                        var attr = manager.Model.FindEntityAttr("Order.ShipRegion");
                        attr.Operations.RemoveByIDs(manager.Model, "StartsWith,Contains");
                        attr.DefaultEditor = new CustomListValueEditor("Lookup", "Lookup");
                    });

                    options.UseSqlFormats(FormatType.Sqlite, formats => {
                        formats.UseDbName = false;
                        formats.UseSchema = false;
                    });
                });

                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

            //Init demo database (if necessary)
            app.EnsureDbInitialized(Configuration, env);
        }
    }
}

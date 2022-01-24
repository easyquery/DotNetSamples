using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using EasyData.Export;

using Korzh.EasyQuery.Services;

using EqDemo.Data;
using EqDemo.Models;
using EqDemo.Services;

namespace EqDemo.Server
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
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("EqDemoDb")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(opts => {
                opts.Password.RequiredLength = 4;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, AppDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddEasyQuery()
                    .UseSqlManager()
                    .AddDefaultExporters()
                    .AddDataExporter<PdfDataExporter>("pdf")
                    .AddDataExporter<ExcelDataExporter>("excel");

            // add default reports generatir
            services.AddScoped<DefaultReportGenerator>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapEasyQuery(options => {
                    options.DefaultModelId = "adhoc-reporting";
                    options.SaveNewQuery = true;
                    options.SaveQueryOnSync = true;
                    options.Endpoint = "/api/adhoc-reporting";

                    options.UseDbContextWithoutIdentity<AppDbContext>(loaderOptions => {
                        //Ignore the "Reports" DbSet as well
                        loaderOptions.AddFilter(entity => {
                            return entity.ClrType != typeof(Report);
                        });
                    });

                    // here we add our custom query store
                    options.UseQueryStore((manager) => new ReportStore(manager.Services));

                    options.UseDefaultAuthProvider((provider) => {
                        //by default NewQuery, SaveQuery and RemoveQuery actions are accessible by the users with 'eq-manager' role 
                        //here you can remove that requirement and make those actions available for all authorized users
                        //provider.RequireAuthorization(EqAction.NewQuery, EqAction.SaveQuery, EqAction.RemoveQuery);

                        //here is an example how you can make some actions accessible only by users with a particular role.
                        //provider.RequireRole(DefaultEqAuthProvider.EqManagerRole, EqAction.NewQuery, EqAction.SaveQuery, EqAction.RemoveQuery);
                    });

                    options.AddPreFetchTunerWithHttpContext((manager, context) => {
                        //the next two lines demonstrate how to add to each generated query a condition that filters data by the current user
                        //string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        //manager.Query.ExtraConditions.AddSimpleCondition("Employees.EmployeeID", "Equal", userId);
                    });
                });
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });

            //Init demo database (if necessary)
            app.EnsureDbInitialized(Configuration, env);
        }
    }
}

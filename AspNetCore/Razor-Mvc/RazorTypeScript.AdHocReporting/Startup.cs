using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using EqDemo.Models;
using EqDemo.Services;

using Korzh.EasyQuery.Services;
using EasyData.Export;

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
            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("EqDemoDb")));
            services.AddDefaultIdentity<IdentityUser>(opts => {
                //Password options
                opts.Password.RequiredLength = 4;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            })
             .AddRoles<IdentityRole>()
             .AddDefaultUI()
             .AddEntityFrameworkStores<AppDbContext>();

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddEasyQuery()
                    .UseSqlManager()
                    .AddDefaultExporters()
                    .AddDataExporter<PdfDataExporter>("pdf")
                    .AddDataExporter<ExcelDataExporter>("excel");

            // add default reports generatir
            services.AddScoped<DefaultReportGenerator>();
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

            //Disable all Identity/Account/Manage actions
            var redirectOptions = new RewriteOptions()
                .AddRedirect("(?i:identity/account/forgotpassword(/.*)?$)", "/")
                .AddRedirect("(?i:identity/account/manage(/.*)?$)", "/");
            app.UseRewriter(redirectOptions);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEasyQuery(options => {
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            //Init demo database (if necessary)
            app.EnsureDbInitialized(Configuration, env);
        }
    }
}

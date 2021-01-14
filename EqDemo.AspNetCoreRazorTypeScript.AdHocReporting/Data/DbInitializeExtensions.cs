using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

using Korzh.DbUtils;

using Korzh.EasyQuery.Services;

namespace EqDemo.Services
{
    public static class DbInitializeExtensions
    {
        public static void EnsureDbInitialized(this IApplicationBuilder app, IConfiguration config, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = scope.ServiceProvider.GetService<AppDbContext>()) {
                if (context.Database.EnsureCreated()) {
                    Korzh.DbUtils.DbInitializer.Create(options => {
                        options.UseSqlServer(config.GetConnectionString("EqDemoDb"));
                        options.UseZipPacker(System.IO.Path.Combine(env.ContentRootPath, "App_Data", "EqDemoData.zip"));
                    })
                    .Seed();
                }

                if (context.Database.CanConnect()) {
                    //create default user
                    CheckAddDefaultUser(scope.ServiceProvider, config);

                    //create eq-manager role
                    CheckAddManagerRole(scope.ServiceProvider);
                }
            }
        }

        const string _defaultUserEmail = "demo@korzh.com";
        const string _defaultUserPassword = "demo";

        private static void CheckAddDefaultUser(IServiceProvider scopedServices, IConfiguration config)
        {
            var manager = scopedServices.GetRequiredService<UserManager<IdentityUser>>();

            try {
                var user = manager.FindByEmailAsync(_defaultUserEmail).Result;
                var resetDemoUser = config.GetValue<bool>("resetDefaultUser");
                if (resetDemoUser && user != null) {
                    var dbContext = scopedServices.GetRequiredService<AppDbContext>();
                    dbContext.Reports.RemoveRange(dbContext.Reports.Where(r => r.OwnerId == user.Id));
                    dbContext.SaveChanges();

                    manager.DeleteAsync(user).GetAwaiter().GetResult();
                    user = null;
                }

                if (user == null) {
                    user = new IdentityUser() {
                        UserName = _defaultUserEmail,
                        Email = _defaultUserEmail,
                        EmailConfirmed = true
                    };
                    var result = manager.CreateAsync(user, _defaultUserPassword).Result;
                    if (result.Succeeded) {
                        var defaultReportsGenerator = scopedServices.GetRequiredService<DefaultReportGenerator>();
                        defaultReportsGenerator.GenerateAsync(user).GetAwaiter().GetResult();
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

        private static void CheckAddManagerRole(IServiceProvider scopedServices)
        {
            var manager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

            try {
                IdentityRole role = manager.FindByNameAsync(DefaultEqAuthProvider.EqManagerRole).Result;
                if (role == null) {
                    role = new IdentityRole(DefaultEqAuthProvider.EqManagerRole);
                    var result = manager.CreateAsync(role).Result;
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }
    }
}

using System;

using Microsoft.AspNetCore.Identity;

using Korzh.DbUtils;

using Korzh.EasyQuery.Services;

namespace EqDemo
{
    public static class DbInitializeExtensions
    {
        public static async Task EnsureDbInitializedAsync(this IApplicationBuilder app, IConfiguration config, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = scope.ServiceProvider.GetService<AppDbContext>()) {
                if (context.Database.EnsureCreated()) {
                    Korzh.DbUtils.DbInitializer.Create(options => {
                        options.UseSqlite(config.GetConnectionString("EqDemoDb"));
                        //options.UseSqlServer(config.GetConnectionString("EqDemoDb"));
                        options.UseZipPacker(System.IO.Path.Combine(env.ContentRootPath, "App_Data", "EqDemoData.zip"));
                    })
                    .Seed();
                }

                if (context.Database.CanConnect()) {
                    //create eq-manager role
                    await CheckAddManagerRoleAsync(scope.ServiceProvider);

                    //create default user
                    await CheckAddDefaultUserAsync(scope.ServiceProvider);
                }
            }
        }

        const string _defaultUserEmail = "demo@korzh.com";
        const string _defaultUserPassword = "demo";

        private static async Task CheckAddDefaultUserAsync(IServiceProvider scopedServices)
        {
            var userManager = scopedServices.GetRequiredService<UserManager<IdentityUser>>();
            try {
                var user = await userManager.FindByEmailAsync(_defaultUserEmail);
                if (user == null) {
                    user = new IdentityUser() {
                        UserName = _defaultUserEmail,
                        Email = _defaultUserEmail,
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(user, _defaultUserPassword);
                    if (result.Succeeded) {
                        await userManager.AddToRoleAsync(user, DefaultEqAuthProvider.EqManagerRole);
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }

        private static async Task CheckAddManagerRoleAsync(IServiceProvider scopedServices)
        {
            var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

            try {
                var role = await roleManager.FindByNameAsync(DefaultEqAuthProvider.EqManagerRole);
                if (role == null) {
                    role = new IdentityRole(DefaultEqAuthProvider.EqManagerRole);
                    await roleManager.CreateAsync(role);
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }
    }
}

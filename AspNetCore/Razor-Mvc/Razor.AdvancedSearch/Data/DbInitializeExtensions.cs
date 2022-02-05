using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using Korzh.DbUtils;

namespace EqDemo.Services
{
    public static class DbInitializeExtensions
    {
        public static void EnsureDbInitialized(this IApplicationBuilder app, string connectionString, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = scope.ServiceProvider.GetService<AppDbContext>()) {
                if (context.Database.EnsureCreated()) {
                    Console.Write("Initializing demo DB...");
                    DbInitializer.Create(options => {
                        options.UseSqlite(connectionString);
                        //options.UseSqlServer(connectionString);
                        options.UseZipPacker(System.IO.Path.Combine(env.ContentRootPath, "App_Data", "EqDemoData.zip"));
                    })
                    .Seed();
                    Console.WriteLine("done");
                }
            }
        }
    }
}

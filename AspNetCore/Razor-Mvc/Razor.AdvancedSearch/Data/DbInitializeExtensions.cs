using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

using Korzh.DbUtils;

namespace EqDemo.Services
{
    public static class DbInitializeExtensions
    {
        public static void EnsureDbInitialized(this IApplicationBuilder app, string connectionString, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            using (var context = scope.ServiceProvider.GetService<AppDbContext>()) {
                // A marker file exists while the DB is being seeded. If it is still there, the previous
                // start stopped half-way (the app was closed or seeding failed), leaving an incomplete DB
                // that EnsureCreated would never seed again - so drop it and start from scratch.
                var seedingMarker = System.IO.Path.Combine(env.ContentRootPath, "eqdemo-sqlite.seeding");
                if (System.IO.File.Exists(seedingMarker)) {
                    var dbFile = context.Database.GetDbConnection().DataSource;
                    context.Database.EnsureDeleted();
                    if (context.Database.IsSqlite()) {
                        System.IO.File.Delete(dbFile + "-wal");
                        System.IO.File.Delete(dbFile + "-shm");
                    }
                }

                if (context.Database.EnsureCreated()) {
                    System.IO.File.WriteAllText(seedingMarker, "");
                    Console.Write("Initializing demo DB...");
                    DbInitializer.Create(options => {
                        options.UseSqlite(connectionString);
                        //options.UseSqlServer(connectionString);
                        options.UseZipPacker(System.IO.Path.Combine(env.ContentRootPath, "App_Data", "EqDemoData.zip"));
                    })
                    .Seed();

                    // SQLite keeps fresh writes in the -wal file until a checkpoint. Move them into the
                    // DB file, so the file is complete on its own - even if copied while the app runs.
                    if (context.Database.IsSqlite())
                        context.Database.ExecuteSqlRaw("PRAGMA wal_checkpoint(TRUNCATE);");
                    System.IO.File.Delete(seedingMarker);
                    Console.WriteLine("done");
                }
            }
        }
    }
}

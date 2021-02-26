namespace EqDemo.Migrations
{
    using Korzh.DbUtils;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EqDemo.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "EqDemo.Models.ApplicationDbContext";
        }

        protected override void Seed(EqDemo.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            Korzh.DbUtils.DbInitializer.Create(options => {
                options.UseSqlServer(context.Database.Connection.ConnectionString);
                options.UseZipPacker(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/EqDemoData.zip"));
            })
            .Seed();
        }
    }
}

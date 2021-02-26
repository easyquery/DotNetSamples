using Microsoft.Owin;
using Owin;

using System.Data.Entity.Migrations;

using EqDemo.Migrations;

[assembly: OwinStartupAttribute(typeof(EqDemo.Startup))]
namespace EqDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            var databaseMigrator = new DbMigrator(new Configuration());
            databaseMigrator.Update();

            IdentityHelper.SeedEqManagerRole();
            IdentityHelper.SeedDefaultUser();
        }
    }
}

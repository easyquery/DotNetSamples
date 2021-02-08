using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.Data.Entity.Migrations;

using EqDemo.Migrations;

namespace EqDemo
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //EasyQuery license keys
            Korzh.EasyQuery.AspNet.License.Key = "M-Vm5PXqfpFr0P6bDruZ2wQIC0HYW2";
            Korzh.EasyQuery.AspNet.JSLicense.Key = "M-Vm5PXqfpFr0P6bDruZ2wBIJ1H334";

            // init db
            var databaseMigrator = new DbMigrator(new Configuration());
            databaseMigrator.Update();
        }
    }
}
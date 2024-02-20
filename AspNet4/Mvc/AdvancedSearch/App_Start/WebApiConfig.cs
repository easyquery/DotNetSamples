using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;
using EasyData.Export;
using Korzh.EasyQuery.Services;

namespace EqDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes(new WebApiCustomDirectRouteProvider());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Register you exporters here
            EasyQueryManager.RegisterExporter("csv", new CsvDataExporter());
            EasyQueryManager.RegisterExporter("excel", new ExcelDataExporter());
            EasyQueryManager.RegisterExporter("pdf", new PdfDataExporter());

            EasyQueryManagerSql.RegisterDbGate<Korzh.EasyQuery.DbGates.SqlClientGate>();
        }
    }

    public class WebApiCustomDirectRouteProvider : DefaultDirectRouteProvider
    {
        protected override IReadOnlyList<IDirectRouteFactory>
            GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
        {
            // inherit route attributes decorated on base class controller's actions
            return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(inherit: true);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using Korzh.EasyQuery.Services;

namespace EqDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var httpControllerRouteHandler = typeof(System.Web.Http.WebHost.HttpControllerRouteHandler).GetField("_instance",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            // To support Session in WebAPI
            if (httpControllerRouteHandler != null)
            {
                httpControllerRouteHandler.SetValue(null,
                    new Lazy<System.Web.Http.WebHost.HttpControllerRouteHandler>(() => new SessionHttpControllerRouteHandler(), true));
            }

            // Web API routes
            config.MapHttpAttributeRoutes(new WebApiCustomDirectRouteProvider());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Register you exportes here
            // to make export works
            EasyQueryManager.RegisterExporter("csv", new CsvDataExporter());
            EasyQueryManager.RegisterExporter("excel-html", new ExcelHtmlDataExporter());

            // Uncomment this line to enable model loading from DbConnection
            // EasyQueryManagerSql.RegisterDbGate<Korzh.EasyQuery.DbGates.SqlServerGate>();
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

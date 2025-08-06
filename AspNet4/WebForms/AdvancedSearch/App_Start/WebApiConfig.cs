using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

using EasyData.Export;

using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.AspNet;

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
            config.MapHttpAttributeRoutesWithEasyQuery();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Register you exportes here
            // to make export works
            EasyQueryManager.RegisterExporter("csv", new CsvDataExporter());
            EasyQueryManager.RegisterExporter("excel", new ExcelDataExporter());
            EasyQueryManager.RegisterExporter("pdf", new PdfDataExporter());

            // Allows to use DbConnectionModelLoader
            EasyQueryManagerSql.RegisterDbGate<Korzh.EasyQuery.DbGates.SqlServerGate>();
        }
    }
}

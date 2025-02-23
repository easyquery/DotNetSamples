using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using Korzh.EasyQuery.Services;

using EasyData.Export;
using Korzh.EasyQuery.AspNet;

namespace EqDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

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
            EasyQueryManager.RegisterExporter("excel-html", new ExcelHtmlDataExporter());
            EasyQueryManager.RegisterExporter("pdf", new PdfDataExporter());

            // Uncomment this line to enable model loading from DbConnection
            EasyQueryManagerSql.RegisterDbGate<Korzh.EasyQuery.DbGates.SqlClientGate>();
        }
    }
}

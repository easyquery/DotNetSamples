Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Web.Http
Imports System.Web.Http.Controllers
Imports System.Web.Http.Routing

Imports EasyData.Export

Imports Korzh.EasyQuery.Services
Imports Korzh.EasyQuery.AspNet

Public Module WebApiConfig
    Public Sub Register(ByVal config As HttpConfiguration)

        ' Web API configuration and services
        ' Web API routes
        ' Dim customRouteProvider As New WebApiCustomDirectRouteProvider
        ' config.MapHttpAttributeRoutes(customRouteProvider)
        config.MapHttpAttributeRoutesWithEasyQuery()

        config.Routes.MapHttpRoute(
            name:="DefaultApi",
            routeTemplate:="api/{controller}/{id}",
            defaults:=New With {.id = RouteParameter.Optional}
        )

        ' Register you exportes here
        ' to make export works
        EasyQueryManager.RegisterExporter("csv", New CsvDataExporter())
        EasyQueryManager.RegisterExporter("excel", New ExcelDataExporter())
        EasyQueryManager.RegisterExporter("pdf", New PdfDataExporter())

        ' Uncomment this line to enable model loading from DbConnection
        ' EasyQueryManagerSql.RegisterDbGate(Of Korzh.EasyQuery.DbGates.SqlServerGate)();

    End Sub
End Module



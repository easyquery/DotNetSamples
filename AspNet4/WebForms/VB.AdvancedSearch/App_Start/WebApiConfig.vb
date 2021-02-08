Imports System.Collections.Generic
Imports System.Web.Http
Imports System.Web.Http.Controllers
Imports System.Web.Http.Routing

Imports Korzh.EasyQuery.Services

Public Module WebApiConfig
    Public Sub Register(ByVal config As HttpConfiguration)

        ' Web API configuration and services
        ' Web API routes
        Dim customRouteProvider As New WebApiCustomDirectRouteProvider
        config.MapHttpAttributeRoutes(customRouteProvider)

        config.Routes.MapHttpRoute(
            name:="DefaultApi",
            routeTemplate:="api/{controller}/{id}",
            defaults:=New With {.id = RouteParameter.Optional}
        )

        ' Register you exportes here
        ' to make export works
        EasyQueryManager.RegisterExporter("csv", New CsvDataExporter())
        EasyQueryManager.RegisterExporter("excel-html", New ExcelHtmlDataExporter())

        ' Uncomment this line to enable model loading from DbConnection
        ' EasyQueryManagerSql.RegisterDbGate(Of Korzh.EasyQuery.DbGates.SqlServerGate)();

    End Sub
End Module

Public Class WebApiCustomDirectRouteProvider
    Inherits DefaultDirectRouteProvider
    Protected Overrides Function GetActionRouteFactories(actionDescriptor As HttpActionDescriptor) As IReadOnlyList(Of IDirectRouteFactory)

        '    '// inherit route attributes decorated on base class controller's actions
        Return actionDescriptor.GetCustomAttributes(Of IDirectRouteFactory)(True)

    End Function

End Class

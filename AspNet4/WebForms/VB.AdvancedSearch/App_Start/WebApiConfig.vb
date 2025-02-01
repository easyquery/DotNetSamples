Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Web.Http
Imports System.Web.Http.Controllers
Imports System.Web.Http.Routing

Imports EasyData.Export

Imports Korzh.EasyQuery.Services

Public Module WebApiConfig
    Public Sub Register(ByVal config As HttpConfiguration)

        ' Web API configuration and services
        ' Web API routes
        ' Dim customRouteProvider As New WebApiCustomDirectRouteProvider
        ' config.MapHttpAttributeRoutes(customRouteProvider)
        config.MapHttpAttributeRoutes()

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

'Public Class WebApiCustomDirectRouteProvider
'    Inherits DefaultDirectRouteProvider
'    Protected Overrides Function GetActionRouteFactories(actionDescriptor As HttpActionDescriptor) As IReadOnlyList(Of IDirectRouteFactory)
'        If TypeOf actionDescriptor Is ReflectedHttpActionDescriptor Then
'            Dim reflectedHttpActionDescriptor = DirectCast(actionDescriptor, ReflectedHttpActionDescriptor)
'            If reflectedHttpActionDescriptor.MethodInfo IsNot Nothing AndAlso reflectedHttpActionDescriptor.MethodInfo.DeclaringType IsNot actionDescriptor.ControllerDescriptor.ControllerType Then
'                Return Nothing
'            End If
'        End If

'        Dim customAttributes As Collection(Of IDirectRouteFactory) = actionDescriptor.GetCustomAttributes(Of IDirectRouteFactory)(inherit:=True)
'        Dim customAttributes2 As Collection(Of IHttpRouteInfoProvider) = actionDescriptor.GetCustomAttributes(Of IHttpRouteInfoProvider)(inherit:=True)
'        Dim list As New List(Of IDirectRouteFactory)()

'        list.AddRange(customAttributes)

'        For Each item As IHttpRouteInfoProvider In customAttributes2
'            If Not TypeOf item Is IDirectRouteFactory Then
'                list.Add(New RouteInfoDirectRouteFactory(item))
'            End If
'        Next

'        Return list

'    End Function

'End Class

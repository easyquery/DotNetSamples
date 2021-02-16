Imports System
Imports System.Configuration
Imports System.Data.Entity.Migrations
Imports System.Web
Imports System.Web.Http
Imports System.Web.Optimization
Imports System.Web.Routing

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)

        ' Fires when the application is started
        GlobalConfiguration.Configure(AddressOf WebApiConfig.Register)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)

        Dim databaseMigrator = New DbMigrator(New EqDemo.Migrations.Configuration())
        databaseMigrator.Update()

    End Sub

End Class
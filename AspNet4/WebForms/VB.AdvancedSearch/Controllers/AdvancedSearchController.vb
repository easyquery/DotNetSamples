Imports System
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Web.Http

Imports Korzh.EasyQuery.AspNet
Imports Korzh.EasyQuery.Services

<RoutePrefix("api/easyquery")>
Public Class AdvancedSearchController : Inherits EasyQueryApiController

    Protected Overrides Sub ConfigureEasyQueryOptions(options As EasyQueryOptions)
        options.UseManager(Of EasyQueryManagerSql)()

        options.DefaultModelId = "nwind"
        options.BuildQueryOnSync = True

        options.UseDbContext(ApplicationDbContext.Create())
        ' Do Not forget to uncomment SqlClientGate registration in WebApiConfig.cs file
        ' options.UseDbConnectionModelLoader();

        Dim path = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data")
        options.UseQueryStore(Function(__) New FileQueryStore(path))
    End Sub
End Class

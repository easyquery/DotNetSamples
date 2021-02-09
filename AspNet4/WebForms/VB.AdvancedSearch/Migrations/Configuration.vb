Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Migrations
Imports System.Linq
Imports Korzh.DbUtils

Namespace Migrations

    Friend NotInheritable Class Configuration 
        Inherits DbMigrationsConfiguration(Of ApplicationDbContext)

        Public Sub New()
            AutomaticMigrationsEnabled = False
            ContextKey = "EqDemo.Models.ApplicationDbContext"
        End Sub

        Protected Overrides Sub Seed(context As ApplicationDbContext)
            '  This method will be called after migrating to the latest version.

            '  You can use the DbSet(Of T).AddOrUpdate() helper extension method 
            '  to avoid creating duplicate seed data.

            DbInitializer.Create(Function(options)
                                     options.UseSqlServer(context.Database.Connection.ConnectionString)
                                     options.UseZipPacker(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/EqDemoData.zip"))
                                 End Function).Seed()
        End Sub

    End Class

End Namespace

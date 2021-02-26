Imports System
Imports System.Data

Imports Newtonsoft.Json.Linq

Imports Korzh.EasyQuery.Services

Public Class CustomEasyQueryManager : Inherits EasyQueryManagerSql
    Public Sub New(services As IServiceProvider, options As EasyQueryOptions)
        MyBase.New(services, options)
    End Sub

    Public Overrides Function GetDataReader(Optional options As JObject = Nothing, Optional addPaging As Boolean = False) As IDataReader

        For Each tuner In PreExecuteTuners
            tuner.Tune(Me)
        Next

        Return MyBase.GetDataReader(options, addPaging)
    End Function

End Class

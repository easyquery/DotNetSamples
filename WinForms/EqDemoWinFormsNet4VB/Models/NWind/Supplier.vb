Imports System.ComponentModel.DataAnnotations.Schema

Public Class Supplier
    <Column("SupplierID")>
    Public Property Id As Integer
    Public Property CompanyName As String
    Public Property ContactName As String
    Public Property ContactTitle As String
    Public Property Address As String
    Public Property City As String
    Public Property Region As String
    Public Property PostalCode As String
    Public Property Country As String
    Public Property Phone As String
    Public Property Fax As String
    Public Property HomePage As String
End Class


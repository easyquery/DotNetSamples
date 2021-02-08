Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports Korzh.EasyQuery

<DisplayColumn("Name")>
<EqEntity(DisplayName:="Client")>
Public Class Customer
    <Column("CustomerID")>
    Public Property Id As String
    <Display(Name:="Company Name")>
    Public Property CompanyName As String
    Public Property Address As String
    Public Property City As String
    Public Property Region As String
    Public Property PostalCode As String
    <EqListValueEditor>
    Public Property Country As String
    Public Property ContactName As String
    Public Property ContactTitle As String
    Public Property Phone As String
    Public Property Fax As String
End Class
Imports System.ComponentModel.DataAnnotations.Schema

Public Class Shipper
    <Column("ShipperID")>
    Public Property Id As Integer
    Public Property CompanyName As String
    Public Property Phone As String
End Class

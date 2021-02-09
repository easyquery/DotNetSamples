Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema


<DisplayColumn("Name")>
Public Class Order
    <DatabaseGenerated(DatabaseGeneratedOption.None)>
    <Column("OrderID")>
    Public Property Id As Integer

    <NotMapped>
    Public ReadOnly Property Name As String
        Get
            Return String.Format("{0:0000}-{1:yyyy-MM-dd}", Me.Id, Me.OrderDate)
        End Get
    End Property

    <Display(Name:="Ordered")>
    Public Property OrderDate As DateTime?
    <Display(Name:="Required")>
    Public Property RequiredDate As DateTime?
    <Display(Name:="Shipped")>
    Public Property ShippedDate As DateTime?
    Public Property Freight As Decimal?
    Public Property CustomerID As String
    <ForeignKey("CustomerID")>
    Public Overridable Property Customer As Customer
    Public Property EmployeeID As Integer?
    <ForeignKey("EmployeeID")>
    Public Overridable Property Employee As Employee
    Public Overridable Property Items As List(Of OrderDetail)
    <ScaffoldColumn(False)>
    Public Property ShipVia As Integer?
    Public Property ShipName As String
    Public Property ShipAddress As String
    Public Property ShipCity As String
    Public Property ShipRegion As String
    Public Property ShipPostalCode As String
    Public Property ShipCountry As String
End Class


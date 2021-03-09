Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<DisplayColumn("Name")>
Public Class Product
    <DatabaseGenerated(DatabaseGeneratedOption.None)>
    <Column("ProductID")>
    Public Property Id As Integer
    <Column("ProductName")>
    Public Property Name As String
    <ScaffoldColumn(False)>
    Public Property SupplierID As Integer?
    <ForeignKey("SupplierID")>
    Public Overridable Property Supplier As Supplier
    Public Property CategoryID As Integer?
    <ForeignKey("CategoryID")>
    Public Overridable Property Category As Category
    Public Property QuantityPerUnit As String
    Public Property UnitPrice As Decimal?
    Public Property UnitsInStock As Short?
    Public Property UnitsOnOrder As Short?
    Public Property ReorderLevel As Short?
    Public Property Discontinued As Boolean
End Class
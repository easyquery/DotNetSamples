Imports System.Data.Entity

Public Class ApplicationDbContext
    Inherits DbContext

    Public Sub New()
        MyBase.New("DefaultConnection")
    End Sub

    Public Property Categories As DbSet(Of Category)
    Public Property Customers As DbSet(Of Customer)
    Public Property Employees As DbSet(Of Employee)
    Public Property Orders As DbSet(Of Order)
    Public Property Products As DbSet(Of Product)
    Public Property OrderDetails As DbSet(Of OrderDetail)
    Public Property Shippers As DbSet(Of Shipper)
    Public Property Suppliers As DbSet(Of Supplier)

    Protected Overrides Sub OnModelCreating(ByVal modelBuilder As DbModelBuilder)
        MyBase.OnModelCreating(modelBuilder)
        modelBuilder.Entity(Of OrderDetail)().ToTable("Order_Details").HasKey(Function(od) New With {od.OrderID, od.ProductID
        })
    End Sub

    Public Shared Function Create() As ApplicationDbContext
        Return New ApplicationDbContext()
    End Function
End Class
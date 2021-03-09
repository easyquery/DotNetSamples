Imports System
Imports System.Data.Entity.Migrations
Imports Microsoft.VisualBasic

Namespace Migrations
    Public Partial Class InitialCreate
        Inherits DbMigration
    
        Public Overrides Sub Up()
            CreateTable(
                "dbo.Categories",
                Function(c) New With
                    {
                        .CategoryID = c.Int(nullable := False),
                        .CategoryName = c.String(),
                        .Description = c.String(),
                        .Array = c.Binary()
                    }) _
                .PrimaryKey(Function(t) t.CategoryID)
            
            CreateTable(
                "dbo.Customers",
                Function(c) New With
                    {
                        .CustomerID = c.String(nullable := False, maxLength := 128),
                        .CompanyName = c.String(),
                        .Address = c.String(),
                        .City = c.String(),
                        .Region = c.String(),
                        .PostalCode = c.String(),
                        .Country = c.String(),
                        .ContactName = c.String(),
                        .ContactTitle = c.String(),
                        .Phone = c.String(),
                        .Fax = c.String()
                    }) _
                .PrimaryKey(Function(t) t.CustomerID)
            
            CreateTable(
                "dbo.Employees",
                Function(c) New With
                    {
                        .EmployeeID = c.Int(nullable := False),
                        .LastName = c.String(nullable := False),
                        .FirstName = c.String(nullable := False),
                        .Title = c.String(maxLength := 30),
                        .TitleOfCourtesy = c.String(),
                        .BirthDate = c.DateTime(),
                        .HireDate = c.DateTime(),
                        .Address = c.String(),
                        .City = c.String(),
                        .Region = c.String(),
                        .PostalCode = c.String(),
                        .Country = c.String(),
                        .HomePhone = c.String(maxLength := 24),
                        .Extension = c.String(maxLength := 4),
                        .Photo = c.Binary(),
                        .PhotoPath = c.String(),
                        .Notes = c.String(),
                        .ReportsTo = c.Int()
                    }) _
                .PrimaryKey(Function(t) t.EmployeeID) _
                .ForeignKey("dbo.Employees", Function(t) t.ReportsTo) _
                .Index(Function(t) t.ReportsTo)
            
            CreateTable(
                "dbo.Orders",
                Function(c) New With
                    {
                        .OrderID = c.Int(nullable := False),
                        .OrderDate = c.DateTime(),
                        .RequiredDate = c.DateTime(),
                        .ShippedDate = c.DateTime(),
                        .Freight = c.Decimal(precision := 18, scale := 2),
                        .CustomerID = c.String(maxLength := 128),
                        .EmployeeID = c.Int(),
                        .ShipVia = c.Int(),
                        .ShipName = c.String(),
                        .ShipAddress = c.String(),
                        .ShipCity = c.String(),
                        .ShipRegion = c.String(),
                        .ShipPostalCode = c.String(),
                        .ShipCountry = c.String()
                    }) _
                .PrimaryKey(Function(t) t.OrderID) _
                .ForeignKey("dbo.Customers", Function(t) t.CustomerID) _
                .ForeignKey("dbo.Employees", Function(t) t.EmployeeID) _
                .Index(Function(t) t.CustomerID) _
                .Index(Function(t) t.EmployeeID)
            
            CreateTable(
                "dbo.Order_Details",
                Function(c) New With
                    {
                        .OrderID = c.Int(nullable := False),
                        .ProductID = c.Int(nullable := False),
                        .UnitPrice = c.Decimal(nullable := False, precision := 18, scale := 2),
                        .Quantity = c.Short(nullable := False),
                        .Discount = c.Single(nullable := False)
                    }) _
                .PrimaryKey(Function(t) New With { t.OrderID, t.ProductID }) _
                .ForeignKey("dbo.Orders", Function(t) t.OrderID, cascadeDelete := True) _
                .ForeignKey("dbo.Products", Function(t) t.ProductID, cascadeDelete := True) _
                .Index(Function(t) t.OrderID) _
                .Index(Function(t) t.ProductID)
            
            CreateTable(
                "dbo.Products",
                Function(c) New With
                    {
                        .ProductID = c.Int(nullable := False),
                        .ProductName = c.String(),
                        .SupplierID = c.Int(),
                        .CategoryID = c.Int(),
                        .QuantityPerUnit = c.String(),
                        .UnitPrice = c.Decimal(precision := 18, scale := 2),
                        .UnitsInStock = c.Short(),
                        .UnitsOnOrder = c.Short(),
                        .ReorderLevel = c.Short(),
                        .Discontinued = c.Boolean(nullable := False)
                    }) _
                .PrimaryKey(Function(t) t.ProductID) _
                .ForeignKey("dbo.Categories", Function(t) t.CategoryID) _
                .ForeignKey("dbo.Suppliers", Function(t) t.SupplierID) _
                .Index(Function(t) t.SupplierID) _
                .Index(Function(t) t.CategoryID)
            
            CreateTable(
                "dbo.Suppliers",
                Function(c) New With
                    {
                        .SupplierID = c.Int(nullable := False, identity := True),
                        .CompanyName = c.String(),
                        .ContactName = c.String(),
                        .ContactTitle = c.String(),
                        .Address = c.String(),
                        .City = c.String(),
                        .Region = c.String(),
                        .PostalCode = c.String(),
                        .Country = c.String(),
                        .Phone = c.String(),
                        .Fax = c.String(),
                        .HomePage = c.String()
                    }) _
                .PrimaryKey(Function(t) t.SupplierID)
            
            CreateTable(
                "dbo.Shippers",
                Function(c) New With
                    {
                        .ShipperID = c.Int(nullable := False, identity := True),
                        .CompanyName = c.String(),
                        .Phone = c.String()
                    }) _
                .PrimaryKey(Function(t) t.ShipperID)
            
        End Sub
        
        Public Overrides Sub Down()
            DropForeignKey("dbo.Order_Details", "ProductID", "dbo.Products")
            DropForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers")
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories")
            DropForeignKey("dbo.Order_Details", "OrderID", "dbo.Orders")
            DropForeignKey("dbo.Orders", "EmployeeID", "dbo.Employees")
            DropForeignKey("dbo.Orders", "CustomerID", "dbo.Customers")
            DropForeignKey("dbo.Employees", "ReportsTo", "dbo.Employees")
            DropIndex("dbo.Products", New String() { "CategoryID" })
            DropIndex("dbo.Products", New String() { "SupplierID" })
            DropIndex("dbo.Order_Details", New String() { "ProductID" })
            DropIndex("dbo.Order_Details", New String() { "OrderID" })
            DropIndex("dbo.Orders", New String() { "EmployeeID" })
            DropIndex("dbo.Orders", New String() { "CustomerID" })
            DropIndex("dbo.Employees", New String() { "ReportsTo" })
            DropTable("dbo.Shippers")
            DropTable("dbo.Suppliers")
            DropTable("dbo.Products")
            DropTable("dbo.Order_Details")
            DropTable("dbo.Orders")
            DropTable("dbo.Employees")
            DropTable("dbo.Customers")
            DropTable("dbo.Categories")
        End Sub
    End Class
End Namespace

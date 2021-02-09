namespace EqDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntitalCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false),
                        CategoryName = c.String(),
                        Description = c.String(),
                        Picture = c.Binary(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerID = c.String(nullable: false, maxLength: 128),
                        CompanyName = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        Region = c.String(),
                        PostalCode = c.String(),
                        Country = c.String(),
                        ContactName = c.String(),
                        ContactTitle = c.String(),
                        Phone = c.String(),
                        Fax = c.String(),
                    })
                .PrimaryKey(t => t.CustomerID);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false),
                        LastName = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        Title = c.String(maxLength: 30),
                        TitleOfCourtesy = c.String(),
                        BirthDate = c.DateTime(),
                        HireDate = c.DateTime(),
                        Address = c.String(),
                        City = c.String(),
                        Region = c.String(),
                        PostalCode = c.String(),
                        Country = c.String(),
                        HomePhone = c.String(maxLength: 24),
                        Extension = c.String(maxLength: 4),
                        Photo = c.Binary(),
                        PhotoPath = c.String(),
                        Notes = c.String(),
                        ReportsTo = c.Int(),
                    })
                .PrimaryKey(t => t.EmployeeID)
                .ForeignKey("dbo.Employees", t => t.ReportsTo)
                .Index(t => t.ReportsTo);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        OrderDate = c.DateTime(),
                        RequiredDate = c.DateTime(),
                        ShippedDate = c.DateTime(),
                        Freight = c.Decimal(precision: 18, scale: 2),
                        CustomerID = c.String(maxLength: 128),
                        EmployeeID = c.Int(),
                        ShipVia = c.Int(),
                        ShipName = c.String(),
                        ShipAddress = c.String(),
                        ShipCity = c.String(),
                        ShipRegion = c.String(),
                        ShipPostalCode = c.String(),
                        ShipCountry = c.String(),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Customers", t => t.CustomerID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .Index(t => t.CustomerID)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.Order_Details",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Short(nullable: false),
                        Discount = c.Single(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderID, t.ProductID })
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        ProductName = c.String(),
                        SupplierID = c.Int(),
                        CategoryID = c.Int(),
                        QuantityPerUnit = c.String(),
                        UnitPrice = c.Decimal(precision: 18, scale: 2),
                        UnitsInStock = c.Short(),
                        UnitsOnOrder = c.Short(),
                        ReorderLevel = c.Short(),
                        Discontinued = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Categories", t => t.CategoryID)
                .ForeignKey("dbo.Suppliers", t => t.SupplierID)
                .Index(t => t.SupplierID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        SupplierID = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        ContactName = c.String(),
                        ContactTitle = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        Region = c.String(),
                        PostalCode = c.String(),
                        Country = c.String(),
                        Phone = c.String(),
                        Fax = c.String(),
                        HomePage = c.String(),
                    })
                .PrimaryKey(t => t.SupplierID);
            
            CreateTable(
                "dbo.Shippers",
                c => new
                    {
                        ShipperID = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.ShipperID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order_Details", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers");
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Order_Details", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Orders", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Orders", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Employees", "ReportsTo", "dbo.Employees");
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropIndex("dbo.Products", new[] { "SupplierID" });
            DropIndex("dbo.Order_Details", new[] { "ProductID" });
            DropIndex("dbo.Order_Details", new[] { "OrderID" });
            DropIndex("dbo.Orders", new[] { "EmployeeID" });
            DropIndex("dbo.Orders", new[] { "CustomerID" });
            DropIndex("dbo.Employees", new[] { "ReportsTo" });
            DropTable("dbo.Shippers");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Products");
            DropTable("dbo.Order_Details");
            DropTable("dbo.Orders");
            DropTable("dbo.Employees");
            DropTable("dbo.Customers");
            DropTable("dbo.Categories");
        }
    }
}

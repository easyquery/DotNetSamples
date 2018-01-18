using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using EqAngularDemo.Models;

namespace EqAngularDemo.Data
{
    public class DbInitializer {

        string _dataPath;
        AppDbContext _dbContext;

        public DbInitializer(AppDbContext context, string dataPath) {
            _dbContext = context;
            _dataPath = dataPath;
        }


        public void CheckDb() {
            if (!CheckDataExistance()) {
                RecreateDbAsync();
            }

        }

        private bool CheckDataExistance() {
            try {
                var testRecord = _dbContext.Customers.FirstOrDefault();

                return testRecord != null;
            }
            catch  {
                return false;
            }

        }

        private void RecreateDbAsync() {
            try {
                _dbContext.Database.EnsureDeleted();
            }
            catch {

            }

            var created = _dbContext.Database.EnsureCreated();
            if (created) {
                Seed();
            }
        }

        public void Seed() {
            LoadCustomers();
            LoadEmployees();
            LoadProducts();
            LoadOrders();
            LoadOrderDetails();
        }

        XElement LoadFile(string fileName) {
            string path = System.IO.Path.Combine(_dataPath, fileName);
            return XDocument.Load(path).Root;
        }

        public void SetIdentityInsert(string tableName, bool on) {
            string onOff = on ? "ON" : "OFF";

            using (var cmd = _dbContext.Database.GetDbConnection().CreateCommand()) {
                cmd.CommandText = $"SET IDENTITY_INSERT {tableName} {onOff}";
                try {
                    cmd.ExecuteNonQuery();
                }
                catch {
                    //ignore if not applied to this table
                    //Console.WriteLine("Can't set identity insert");
                }
            }
        }

        void LoadCustomers() {
            var tableName = "Customers";
            SetIdentityInsert(tableName, true);
            XElement root = LoadFile("Customers.xml");
            foreach (XElement element in root.Elements("Result")) {
                Customer customer = new Customer {
                    Id = element.StringValue("CustomerID"),
                    City = element.StringValue("City"),
                    CompanyName = element.StringValue("CompanyName"),
                    ContactName = element.StringValue("ContactName"),
                    ContactTitle = element.StringValue("ContactTitle"),
                    Phone = element.StringValue("Phone"),
                    Fax = element.StringValue("Fax"),
                    Street = element.StringValue("Address"),
                    PostalCode = element.StringValue("PostalCode"),
                    Region = element.StringValue("Region"),
                    Country = element.StringValue("Country")
                };
                _dbContext.Customers.Add(customer);
            }
            _dbContext.SaveChanges();
            SetIdentityInsert(tableName, false);
        }

        void LoadProducts() {
            XElement root = LoadFile("Products.xml");          
            foreach (XElement element in root.Elements("Result")) {
                Product product = new Product {
                    Id = element.IntValue("ProductID"),
                    Discontinued = element.BoolValue("Discontinued"),
                    Name = element.StringValue("ProductName"),
                    EnglishName = element.StringValue("ProductName"),
                    QuantityPerUnit = element.StringValue("QuantityPerUnit"),
                    ReorderLevel = element.ShortValue("ReorderLevel"),
                    UnitPrice = element.DecimalValue("UnitPrice"),
                    UnitsInStock = element.ShortValue("UnitsInStock"),
                    UnitsOnOrder = element.ShortValue("UnitsOnOrder"),
                };
                _dbContext.Products.Add(product);
            }
            _dbContext.SaveChanges();
        }

        void LoadEmployees() {
            var tableName = "dbo.Employees";
            SetIdentityInsert(tableName, true);

            Dictionary<int, int> d = new Dictionary<int, int>();
            XElement root = LoadFile("Employees.xml");
            foreach (XElement element in root.Elements("Result")) {
                Employee employee = new Employee {
                    Id = element.IntValue("EmployeeID"),
                    LastName = element.StringValue("LastName"),
                    FirstName = element.StringValue("FirstName"),
                    Title = element.StringValue("Title"),
                    BirthDate = element.DateTimeValue("BirthDate"),
                    HireDate = element.DateTimeValue("HireDate"),
                    Street = element.StringValue("Address"),
                    City = element.StringValue("City"),
                    PostalCode = element.StringValue("PostalCode"),
                    Country = element.StringValue("Country"),
                    Region = element.StringValue("Region"),

                    Notes = element.StringValue("Notes"),
                };
                d[employee.Id] = element.IntValue("ReportsTo");
                _dbContext.Employees.Add(employee);
                _dbContext.SaveChanges();
            }

            foreach (Employee emp in _dbContext.Employees) {
                emp.ReportsTo = d[emp.Id];
            }

            _dbContext.SaveChanges();
            SetIdentityInsert(tableName, false);
        }

        void LoadOrders() {
            XElement root = LoadFile("Orders.xml");
            int productId = 0;
            foreach (XElement element in root.Elements("Result")) {
                string customerId = element.StringValue("CustomerID");
                var customer = _dbContext.Customers.Find(customerId);
                //string productId = element.StringValue("CustomerID");
                var product = _dbContext.Products.Find(++productId);
                Order order = new Order {
                    Id = element.IntValue("OrderID"),
                    Customer = customer,
                    Product = product,
                    EmployeeId = element.IntValue("EmployeeID"),
                    OrderDate = element.DateTimeValue("OrderDate"),
                    RequiredDate = element.DateTimeValue("RequiredDate"),
                    ShippedDate = element.DateTimeValue("ShippedDate"),
                    Freight = element.DecimalValue("Freight"),
                    ShipVia = element.IntValue("ShipVia"),
                    ShipName = element.StringValue("ShipName"),
                    ShipAddress = element.StringValue("ShipAddress"),
                    ShipCity = element.StringValue("ShipCity"),
                    ShipPostalCode = element.StringValue("ShipPostalCode"),
                    ShipCountry = element.StringValue("ShipCountry")
                };
                _dbContext.Orders.Add(order);
            }
            _dbContext.SaveChanges();
        }

        void LoadOrderDetails() {
            XElement root = LoadFile("Order_Details.xml");
            foreach (XElement element in root.Elements("Result")) {
                int orderId = element.IntValue("OrderID");
                var order = _dbContext.Orders.Find(orderId);
                int productId = element.IntValue("ProductID");
                var product = _dbContext.Products.Find(productId);
                OrderDetail orderDetail = new OrderDetail {
                    Order = order,
                    Product = product,
                    UnitPrice = element.DecimalValue("UnitPrice"),
                    Quantity = element.ShortValue("Quantity"),
                    Discount = element.FloatValue("Discount")
                };
                _dbContext.OrderDetails.Add(orderDetail);
            }
            _dbContext.SaveChanges();
        }

    }


    public static class Extensions
    {
        public static string StringValue(this XElement element, string name) {
            XElement child = element.Element(name);
            return child == null ? string.Empty : child.Value;
        }

        public static int IntValue(this XElement element, string name) {
            XElement child = element.Element(name);
            if (child == null)
                return 0;
            int result = 0;
            int.TryParse(child.Value, out result);
            return result;
        }

        public static bool BoolValue(this XElement element, string name) {
            XElement child = element.Element(name);
            if (child == null)
                return false;
            bool result = false;
            bool.TryParse(child.Value, out result);
            return result;
        }

        public static decimal DecimalValue(this XElement element, string name) {
            XElement child = element.Element(name);
            return child == null ? 0 : decimal.Parse(child.Value, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static short ShortValue(this XElement element, string name) {
            XElement child = element.Element(name);
            return child == null ? (short)0 : short.Parse(child.Value);
        }

        public static float FloatValue(this XElement element, string name) {
            XElement child = element.Element(name);
            return child == null ? 0 : float.Parse(child.Value, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static DateTime DateTimeValue(this XElement element, string name) {
            XElement child = element.Element(name);
            if (child == null)
                return DateTime.Now;
            DateTime result = DateTime.Now;
            DateTime.TryParse(child.Value, out result);
            return result;
        }

    }


}
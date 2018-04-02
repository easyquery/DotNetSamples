using EqAzureDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EqAzureDemo.Data
{
    public class DbInitializer {

        private readonly string _dataPath;
        private readonly NwindContext _context;

        public DbInitializer(NwindContext context, string dataPath) {
            _context = context;
            _dataPath = dataPath;
        }

        public void CheckDb() {
            if (!CheckDataExistance()) {
                Seed();
            }
        }

        private bool CheckDataExistance() {
            try {
                var testRecord = _context.Customers.GetAll();

                return testRecord.Count() != 0;
            }
            catch {
                return false;
            }

        }

        private void Seed() {
            LoadCustomers();
        }

        private void LoadCustomers() {
            XElement root = LoadFile("Customers.xml");
            foreach (XElement element in root.Elements("Result")) {
                Customer customer = new Customer {
                    Id = element.StringValue("CustomerID"),
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
                _context.Customers.Add(customer);
            }
        }

        private XElement LoadFile(string fileName) {
            string path = System.IO.Path.Combine(_dataPath, fileName);
            return XDocument.Load(path).Root;
        }
    }

    public static class Extensions {
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

        public static float FloatValue(this XElement element, string name) {
            XElement child = element.Element(name);
            return child == null ? 0 : float.Parse(child.Value, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static short ShortValue(this XElement element, string name) {
            XElement child = element.Element(name);
            return child == null ? (short)0 : short.Parse(child.Value);
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

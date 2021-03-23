using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Korzh.EasyQuery;
namespace EqDemo.Models
{
    [DisplayColumn("Name")]
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("OrderID")]
        [EqEntityAttr(DisplayName = "ID", DisplayFormat = "{0:D8}")]
        public int Id { get; set; }
       
        [EqEntityAttr(DisplayName = "Ordered", DisplayFormat = "{0:d}")]
        public DateTime? OrderDate { get; set; }

        [EqEntityAttr(false)]
        public DateTime? RequiredDate { get; set; }

        [EqEntityAttr(DisplayName = "Shipped", DisplayFormat = "{0:d}")]
        public DateTime? ShippedDate { get; set; }

        public decimal? Freight { get; set; }

        public string CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        public int? EmployeeID { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        public virtual List<OrderDetail> Items { get; set; }

        [ScaffoldColumn(false)]
        public int? ShipVia { get; set; }

        public string ShipName { get; set; }

        public string ShipAddress { get; set; }

        public string ShipCity { get; set; }

        public string ShipRegion { get; set; }

        public string ShipPostalCode { get; set; }

        public string ShipCountry { get; set; }
    }
}
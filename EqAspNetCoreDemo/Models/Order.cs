﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Korzh.EasyQuery;
namespace EqAspNetCoreDemo.Models
{
    [DisplayColumn("Name")]
    public class Order {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderID { get; set; }

        [NotMapped]
        public string Name {
            get {
                return string.Format("{0:0000}-{1:yyyy-MM-dd}", this.OrderID, this.OrderDate);
            }
        }
        
        [Display(Name = "Ordered")]
        public DateTime? OrderDate { get; set; }

        [Display(Name = "Required")]
        public DateTime? RequiredDate { get; set; }

        [Display(Name = "Shipped")]
        public DateTime? ShippedDate { get; set; }

        public decimal? Freight { get; set; }

        public virtual Customer Customer { get; set; }

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

        public virtual Product Product { get; set; }

        public int EmployeeId { get; internal set; }
    }


}
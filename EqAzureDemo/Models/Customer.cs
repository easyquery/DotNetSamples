using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

using Korzh.EasyQuery;

namespace EqAzureDemo.Models
{
    public class Customer : TableEntity {

        public Customer(){
            PartitionKey = "Customer";
            ETag = "*";
        }
        
        [IgnoreProperty]
        public string Id {
            get { return RowKey; }
            set { RowKey = value; }
        }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string ContactName { get; set; }

        public string ContactTitle { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }


    }
}

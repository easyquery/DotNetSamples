using System.ComponentModel.DataAnnotations;
using Korzh.EasyQuery;

namespace EasyReportDemo.Models
{

    [DisplayColumn("Name")]
    [EqEntity(DisplayName = "Client")]
    public class Customer {
        public string Id { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        
        public string Street { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        [EqListValueEditor]
        public string Country { get; set; }

        public string ContactName { get; set; }

        public string ContactTitle { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

    }



}
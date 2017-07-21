using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EasyReportDemo.Models
{
    public class Report {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        ApplicationUser User { get; set; }

        string QueryXML { get; set; }
    }
}

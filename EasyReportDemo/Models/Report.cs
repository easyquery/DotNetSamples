using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EasyReportDemo.Models
{
    public class Report {

        public string Id { get; set; }

        //User Id from AspNetUser Table 
        public string UserId { get; set; }

        public string Name { get; set; }

        public string QueryXML { get; set; }
    }
}

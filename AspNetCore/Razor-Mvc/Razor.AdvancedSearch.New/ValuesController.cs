using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Korzh.EasyQuery.Services;

namespace EqDemo.AspNetCoreRazor.AdvancedSearch
{
    [Route("api/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("list1")]
        public IEnumerable<ListItem> GetList1()
        {
            return new List<ListItem> {
                new ListItem("1", "AAAAA"),
                new ListItem("2", "BBBBB"),
                new ListItem("3", "CCCCC")
            };
        }
    }
}

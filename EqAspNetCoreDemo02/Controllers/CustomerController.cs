using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.Linq;

using Korzh.EasyQuery.AspNetCore.Demo02.Data;
using Korzh.EasyQuery.AspNetCore.Demo02.Models;

namespace Korzh.EasyQuery.AspNetCore.Demo02.Controllers
{
    public class CustomerController : Controller {

        AppDbContext dbContext;

        public CustomerController(AppDbContext context) {
            this.dbContext = context;
        }

        // GET: /Customer/
        public IActionResult Index() {
            return View("Customers");
        }


        /// <summary>
        /// This action is called when user clicks on "Apply" button in FilterBar or other data-filtering widget
        /// </summary>
        /// <param name="jsonDict"></param>
        /// <returns>IActionResult which contains a partial view with the filtered result set</returns>
        [HttpPost]
        public IActionResult ApplyTextSearch([FromBody] JsonDict jsonDict) {
            string text = jsonDict["text"] as string;
            var optionsDict = jsonDict["options"] as JsonDict;

            var lvo = optionsDict.ToListViewOptions();

            IPagedList<Customer> list = null;

            if (!string.IsNullOrEmpty(text)) {
                list = dbContext.Customers.FullTextSearchQuery<Customer>(text).ToPagedList(lvo.PageIndex, 20);
            }
            else {
                list = dbContext.Customers.ToPagedList(lvo.PageIndex, 20);
            }

            return View("_CustomerListPartial", list);
        }
    }
}

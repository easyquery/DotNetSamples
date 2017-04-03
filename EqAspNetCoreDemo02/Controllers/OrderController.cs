using System;
using System.Collections.Generic;

using System.Linq;

using System.Net;
using Microsoft.AspNetCore.Mvc;


using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.Linq;
using Korzh.EasyQuery.AspNetCore;

using Korzh.EasyQuery.AspNetCore.Demo02.Models;
using Korzh.EasyQuery.AspNetCore.Demo02.Data;
using Microsoft.EntityFrameworkCore;

namespace Korzh.EasyQuery.AspNetCore.Demo02.Controllers
{
    public class OrderController : Controller
    {
        EqServiceProviderDb eqService;
        AppDbContext dbContext;

        public OrderController(AppDbContext context) {
            this.dbContext = context;

            eqService = new EqServiceProviderDb();



            eqService.ModelLoader = (model, modelName) => {
                model.LoadFromType(typeof(Order));
                model.SortEntities();
            };
            
        }

        // GET: /Order/
        public IActionResult Index() {
            //var orders = db.Orders.Include(o => o.Customer).Include(o => o.Employee);
            return View("Orders");
        }

        public IActionResult GetModel(string modelName) {
            var model = eqService.GetModel();
            return Json(model.SaveToDictionary());
        }

        public IActionResult GetQuery(string queryId) {
            var query = eqService.GetQuery(queryId);

            return Json(query.SaveToDictionary());
        }

        public IActionResult ApplyFilter(string queryJson, string optionsJson) {
            var query = eqService.LoadQueryDict(queryJson.JsonToDictionary());
            var lvo = optionsJson.ToListViewOptions();

            var list = dbContext.Orders
                .Include(c => c.Customer)
                .Include(c => c.Employee)
                .DynamicQuery<Order>(query, lvo.SortBy).ToPagedList(lvo.PageIndex, 20);

            return View("_OrderListPartial", list);

        }



        /// <summary>
        /// This action returns a custom list by different list request options (list name).
        /// </summary>
        /// <param name="options">List request options - an instance of <see cref="ListRequestOptions"/> type.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetList(ListRequestOptions options) {
            return Json(eqService.GetList(options )); //dbContext.Orders
        }

    }
}

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
                model.LoadFromEntityType(typeof(Order));
                model.SortEntities();
            };
            
        }

        // GET: /Order/
        public IActionResult Index() {
            return View("Orders");
        }

        /// <summary>
        /// Gets the model by its ID
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns><see cref="IActionResult"/> object with JSON representation of the model</returns>
        [HttpPost]
        public IActionResult GetModel([FromBody] JsonDict jsonDict) {
            string modelId = jsonDict["modelId"].ToString();
            var model = eqService.GetModel(modelId);
            return Json(model.SaveToJsonDict());
        }


        /// <summary>
        /// Gets the query by its ID
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns><see cref="IActionResult"/> object with JSON representation of the query</returns>
        [HttpPost]
        public IActionResult GetQuery([FromBody] JsonDict jsonDict) {
            var query = eqService.GetQueryByJsonDict(jsonDict);
            return Json(query.SaveToJsonDict());
        }


        /// <summary>
        /// This action returns a custom list by different list request options (list name).
        /// </summary>
        /// <param name="jsonDict">GetList request options.</param>
        /// <returns><see cref="IActionResult"/> object</returns>
        [HttpPost]
        public IActionResult GetList([FromBody] JsonDict jsonDict) {
            return Json(eqService.GetList(jsonDict, dbContext.Orders));
        }


        /// <summary>
        /// This action is called when user clicks on "Apply" button in FilterBar or other data-filtering widget
        /// </summary>
        /// <param name="jsonDict"></param>
        /// <returns>IActionResult which contains a partial view with the filtered result set</returns>
        public IActionResult ApplyQueryFilter([FromBody] JsonDict jsonDict) {
            var queryDict = jsonDict["query"] as JsonDict;
            var optionsDict = jsonDict["options"] as JsonDict;
            var query = eqService.GetQueryByJsonDict(queryDict);

            var lvo = optionsDict.ToListViewOptions();

            var list = dbContext.Orders
                .Include(c => c.Customer)
                .Include(c => c.Employee)
                .DynamicQuery<Order>(query, lvo.SortBy).ToPagedList(lvo.PageIndex, 20);

            return View("_OrderListPartial", list);
        }


    }
}

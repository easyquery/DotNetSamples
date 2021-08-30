using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.Linq;
using Korzh.EasyQuery.AspNetCore;

using EqDemo.Models;

namespace EqDemo.Controllers
{
    [Route("data-filtering")]
    public class OrderController : Controller
    {
        EasyQueryManagerLinq<Order> _eqManager;
        AppDbContext _dbContext;

        public OrderController(IServiceProvider services, AppDbContext dbContext)
        {
            _dbContext = dbContext;

            var options = new EasyQueryOptions();
            options.UseEntity((_) =>
                _dbContext
                    .Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Employee)
                    .AsQueryable());

            _eqManager = new EasyQueryManagerLinq<Order>(options, services);
        }

        // GET: /Order/
        public IActionResult Index()
        {
            return View("Orders");
        }

        /// <summary>
        /// Gets the model by its ID
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns><see cref="IActionResult"/> object with JSON representation of the model</returns>
        [HttpGet("models/{modelId}")]
        public async Task<IActionResult> GetModelAsync(string modelId)
        {
            var model = await _eqManager.GetModelAsync(modelId);

            return this.EqOk(new { Model = model });
        }

        /// <summary>
        /// This action returns a custom list by different list request options (list name).
        /// </summary>
        /// <param name="jsonDict">GetList request options.</param>
        /// <returns><see cref="IActionResult"/> object</returns>
        [HttpGet("models/{modelId}/valuelists/{editorId}")]
        public async Task<IActionResult> GetListAsync(string modelId, string editorId)
        {
            var list = await _eqManager.GetValueListAsync(modelId, editorId);

            return this.EqOk(new { Values = list });
        }

        /// <summary>
        /// This action is called when user clicks on "Apply" button in FilterBar or other data-filtering widget
        /// </summary>
        /// <returns>IActionResult which contains a partial view with the filtered result set</returns>
        [HttpPost("models/{modelId}/fetch")]
        public async Task<IActionResult> ApplyQueryFilter(string modelId, string queryId)
        {
            await _eqManager.ReadRequestContentFromStreamAsync(modelId, Request.Body);

            var orders = _dbContext.Orders
                      .Include(o => o.Customer)
                      .Include(o => o.Employee)
                      .AsQueryable();


            var text = _eqManager.ClientData.TryGetValue("text", out var textObj) ? textObj.ToString() : "";

            //read full-text search text
            if (!string.IsNullOrEmpty(text)) {
                var options = new FullTextSearchOptions
                {
                    Depth = 2,

                    // filter to use search for string fields we chose
                    Filter = (propInfo) =>
                    {
                        if (propInfo.DeclaringType == typeof(Order))
                        {
                            return propInfo.PropertyType == typeof(Customer) || propInfo.PropertyType == typeof(Employee);
                        }

                        if (propInfo.PropertyType == typeof(string)) {
                            if (propInfo.DeclaringType == typeof(Customer)) {
                                return propInfo.Name == "Country" || propInfo.Name == "ContactName";
                            }

                            if (propInfo.DeclaringType == typeof(Employee)) {
                                return propInfo.Name == "FirstName" || propInfo.Name == "LastName";
                            }
                        }

                        return false;
                    }
                };

                orders = orders.FullTextSearchQuery<Order>(text, options);
            }


            orders = orders.DynamicQuery<Order>(_eqManager.Query);

            var list = orders.ToPagedList(_eqManager.Chunk.Page, 15);
            ViewData["Text"] = text;

            return View("_OrderListPartial", list);
        }
    }
}

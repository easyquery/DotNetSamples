
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.Linq;
using Korzh.EasyQuery.AspNet;

using EqDemo.Models;

namespace EqDemo.Controllers
{
    [RoutePrefix("data-filtering")]
    public class OrderController : Controller
    {
        EasyQueryManagerLinq<Order> _eqManager;
        ApplicationDbContext _dbContext;

        public OrderController()
        {
            _dbContext = ApplicationDbContext.Create();
            var services = new EmptyServiceProvider();

            var options = new EasyQueryOptions(services);
            options.UseEntity((_) => _dbContext.Orders);

            //create EasyQuery manager which generates LINQ queries
            _eqManager = new EasyQueryManagerLinq<Order>(options);
        }

        // GET
        [Route]
        public ActionResult Index()
        {
            return View("Orders");
        }

        /// <summary>
        /// Gets the model by its ID
        /// </summary>
        /// <param name="modelId">The ID of the model that will be loaded</param>
        /// <returns><see cref="ActionResult"/> An ActionResult object with JSON representation of the model</returns>
        [HttpGet]
        [Route("models/{modelId}")]
        public async Task<ActionResult> GetModelAsync(string modelId)
        {
            var model = await _eqManager.GetModelAsync(modelId);
            return this.EqOk(new { model }); 
        }

        /// <summary>
        /// This action returns a list values for specified value editor.
        /// </summary>
        /// <param name="modelId">The ID of the model the value editor belongs to.</param>
        /// <param name="modelId">The ID of the value editor. It can be any LIST value editor (SQL LIST, CONST LIST, etc).</param>
        /// <returns>A <see cref="ActionResult"/> object with a JSON represantation of the list.</returns>
        [HttpGet]
        [Route("models/{modelId}/valuelists/{editorId}")]
        public async Task<ActionResult> GetList(string modelId, string editorId)
        {
            var list = await _eqManager.GetValueListAsync(modelId, editorId);

            return this.EqOk(new { values = list });
        }

        /// <summary>
        /// This action is called when user clicks on "Apply" button in FilterBar or other data-filtering widget
        /// </summary>
        /// <returns>An ActionResult object that contains a partial view with the filtered result set.</returns>
        [HttpPost]
        [Route("models/{modelId}/fetch")]
        public async Task<ActionResult> ApplyQueryFilter(string modelId)
        {
            Request.InputStream.Position = 0;
            await _eqManager.ReadRequestContentFromStreamAsync(modelId, Request.InputStream);

            var orders = _dbContext.Orders
                      .Include(o => o.Customer)
                      .Include(o => o.Employee)
                      .AsQueryable();

            var clientData = (IDictionary<string, object>)_eqManager.ClientData;
            var text = clientData != null && clientData.TryGetValue("text", out object value)
                ? (string)value : "";

            //read full-text search text
            if (!string.IsNullOrEmpty(text)) {
                var options = new FullTextSearchOptions
                {
                    Depth = 2,

                    // filter to use search for string fields we chose
                    Filter = (propInfo) =>
                    {
                        if (propInfo.DeclaringType == typeof(Order)) {
                            return propInfo.PropertyType == typeof(Customer) || propInfo.PropertyType == typeof(Employee);
                        }
                        else if (propInfo.PropertyType == typeof(string)) {
                            if (propInfo.DeclaringType == typeof(Customer)) {
                                return propInfo.Name == "CompanyName" || propInfo.Name == "Country";
                            }
                            else if (propInfo.DeclaringType == typeof(Employee)) {
                                return propInfo.Name == "FirstName" || propInfo.Name == "LastName";
                            }
                        }
                        return false;
                    }
                };

                orders = orders.FullTextSearchQuery<Order>(text, options);
            }


            orders = orders
             .DynamicQuery<Order>(_eqManager.Query);

            var list = orders.ToPagedList(_eqManager.Chunk.Page, 15);
            ViewData["Text"] = text;

            return View("_OrderListPartial", list);
        }
    }
}
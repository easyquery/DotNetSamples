using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json.Linq;

using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.Linq;
using Korzh.EasyQuery.AspNetCore;
using Korzh.EasyQuery.EntityFrameworkCore;

using EqAspNetCoreDemo.Models;
using Newtonsoft.Json;

namespace EqAspNetCoreDemo.Controllers
{    
    [Route("data-filtering")]
    public class OrderController : Controller
    {
        EasyQueryManagerLinq<Order> _eqManager;
        AppDbContext _dbContext;

        public OrderController(IServiceProvider services, AppDbContext dbContext) {
            this._dbContext = dbContext;

            var options = new EasyQueryOptions(services);
            options.UseEntity((_, __) => _dbContext
                                            .Orders
                                            .Include(o => o.Customer)
                                            .Include(o => o.Employee)
                                            .AsQueryable());

            _eqManager = new EasyQueryManagerLinq<Order>(services, options);           
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
        public async Task<IActionResult> GetModel(string modelId)
        {
            var model = _eqManager.GetModel(modelId);
            var modelJson = await model.SaveToJsonStringForClientAsync();
            return Ok("{\"result\":\"ok\", \"model\":" + modelJson + "}");
        }

        /// <summary>
        /// This action returns a custom list by different list request options (list name).
        /// </summary>
        /// <param name="jsonDict">GetList request options.</param>
        /// <returns><see cref="IActionResult"/> object</returns>
        [HttpGet("models/{modelId}/valuelists/{editorId}")]
        public IActionResult GetList(string modelId, string editorId)
        {
            var list = _eqManager.GetValueList(modelId, editorId);
            var valuesJson = JsonConvert.SerializeObject(list);
            return Ok("{\"result\":\"ok\", \"values\":" + valuesJson + "}");
        }

        /// <summary>
        /// This action is called when user clicks on "Apply" button in FilterBar or other data-filtering widget
        /// </summary>
        /// <param name="jsonDict"></param>
        /// <returns>IActionResult which contains a partial view with the filtered result set</returns>
        [HttpPost("models/{modelId}/queries/{queryId}/execute")]
        public IActionResult ApplyQueryFilter(string modelId, string queryId, [FromBody] JObject jObject) {
            var query = _eqManager.LoadQueryWithOptionsFromJson(modelId, jObject);

            var queryable = _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .DynamicQuery<Order>(query);

            var list = queryable.ToPagedList(_eqManager.Paging.PageIndex, 15);

            return View("_OrderListPartial", list);
        }
    }
}

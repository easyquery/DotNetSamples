using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Korzh.EasyQuery;
using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.AspNetCore;
using Korzh.EasyQuery.Linq;

using EqAzureDemo.Data;
using EqAzureDemo.Models;

namespace EqAzureDemo.Controllers
{
    public class CustomerController: Controller {
        private readonly EqServiceProviderDb _eqService;
        private readonly NwindContext _context;

        public CustomerController(NwindContext context) {
           _context = context;


            _eqService = new EqServiceProviderDb();

            _eqService.ModelLoader = (model, modelName) => {
                model.LoadFromEntityType(typeof(Customer));
                model.SortEntities();

                //These operators are not supported in OData queries - so we need to remove them
                model.Operators.RemoveByIDs(model, "InList,NotInList,ListContains,Contains,NotContains");
            };

        }

        // GET: /Order/
        public IActionResult Index() {
            return View("Customers");
        }

        /// <summary>
        /// Gets the model by its ID
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns><see cref="IActionResult"/> object with JSON representation of the model</returns>
        [HttpPost]
        public IActionResult GetModel([FromBody] JsonDict jsonDict) {
            string modelId = jsonDict["modelId"].ToString();
            var model = _eqService.GetModel(modelId);
            return Json(model.SaveToJsonDict());
        }

        /// <summary>
        /// Gets the query by its ID
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns><see cref="IActionResult"/> object with JSON representation of the query</returns>
        [HttpPost]
        public IActionResult GetQuery([FromBody] JsonDict jsonDict) {
            var query = _eqService.GetQueryByJsonDict(jsonDict);
            return Json(query.SaveToJsonDict());
        }

        /// <summary>
        /// This action returns a custom list by different list request options (list name).
        /// </summary>
        /// <param name="jsonDict">GetList request options.</param>
        /// <returns><see cref="IActionResult"/> object</returns>
        [HttpPost]
        public IActionResult GetList([FromBody] JsonDict jsonDict) {
            return Json(_eqService.GetList(jsonDict, _context.Customers.GetAll() as IQueryable<Customer>));
        }


        /// <summary>
        /// This action is called when user clicks on "Apply" button in FilterBar or other data-filtering widget
        /// </summary>
        /// <param name="jsonDict"></param>
        /// <returns>IActionResult which contains a partial view with the filtered result set</returns>
        [HttpPost]
        public IActionResult ApplyQueryFilter([FromBody] JsonDict jsonDict) {
            var queryDict = jsonDict["query"] as JsonDict;
            var optionsDict = jsonDict["options"] as JsonDict;
            var query = _eqService.GetQueryByJsonDict(queryDict);

            var lvo = optionsDict.ToListViewOptions();

            //We can use OData query to get filtered information from Azure Storage
            var odataBuilder = new ODataQueryBuilder(query);
            string filterString = odataBuilder.Build();

            var list = _context.Customers.Filter(filterString);

            // Here is another option of solving the same task
            // In this case we don't build OData query, so we can use all operators in our model. 
            // However, it will work well only if Customers implements IQueryable interface 
            // Otherwise it will load the whole table content from the storage before applying the query.
            //
            // list = _context.Customers.GetAll().DynamicQuery<Customer>(query).OrderBy(d => d.Id);
            // 

            return View("_CustomerListPartial", list);
        }
    }
}

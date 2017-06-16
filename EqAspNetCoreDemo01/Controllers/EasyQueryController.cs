using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;


using Microsoft.AspNetCore.Mvc;

using System.Data.SqlClient;

using Korzh.EasyQuery.Db;
using Korzh.EasyQuery.Services;

using Korzh.EasyQuery.AspNetCore;


namespace Korzh.EasyQuery.AspNetCore.Demo01
{

    public class EasyQueryController : Controller {

        private EqServiceProviderDb eqService;

        public EasyQueryController(IHostingEnvironment env, IConfiguration config) {
            eqService = new EqServiceProviderDb();
            eqService.DefaultModelId = "NWindSQL";            

            eqService.CacheGetter = (key) => HttpContext.Session.GetString(key);
            eqService.CacheSetter = (key, value) => HttpContext.Session.SetString(key, value);

            eqService.StoreQueryInCache = true;

            eqService.Formats.SetDefaultFormats(FormatType.MsSqlServer);
            eqService.Formats.UseSchema = true;

            string dataPath = System.IO.Path.Combine(env.ContentRootPath , "App_Data");
            eqService.DataPath = dataPath;

            eqService.ConnectionResolver = () => {
                return new SqlConnection(config.GetConnectionString("EqDemoDb"));
            };


            eqService.ValueListResolver = (listname) => {
                if (listname == "Regions") {
                    return new List<ListItem> {
                        new ListItem("US", "USA", new List<ListItem> {
                            new ListItem("CA","California"),
                		    new ListItem("CO", "Colorado"),
                            new ListItem("OR","Oregon"),
                		    new ListItem("WA", "Washington")
                        }),

                        new ListItem("CA", "CANADA", new List<ListItem> {
                            new ListItem("AB","Alberta"),
                		    new ListItem("ON", "Ontario"),
                		    new ListItem("QC", "Québec")
                        }),

                    };
                }
                return Enumerable.Empty<ListItem>();
            };

        }

        public ActionResult Index(string queryId) {
            ViewData["QueryId"] = queryId ?? "";
            return View("EasyQuery");
        }



        #region public actions
        /// <summary>
        /// Gets the model by its ID
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns></returns>
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
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetQuery([FromBody] JsonDict jsonDict) {
            var query = eqService.GetQueryByJsonDict(jsonDict);
            return Json(query.SaveToJsonDict());
        }


        /// <summary>
        /// Saves the query passed in request with new name
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns>IActionResult object</returns>
        [HttpPost]
        public IActionResult SaveQuery([FromBody] JsonDict jsonDict) {
            var queryDict = jsonDict["query"] as JsonDict;
            var queryName = jsonDict["queryName"].ToString();

            eqService.SaveQueryDict(queryDict, queryName);

            JsonDict dict = new JsonDict();
            dict.Add("result", "OK");
            return Json(dict);
        }


        [HttpPost]
        /// <summary>
        /// Gets the list of saved queries 
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns>IActionResult object</returns>
        public JsonResult GetQueryList([FromBody] JsonDict jsonDict) {
            string modelId = jsonDict["modelId"].ToString();
            var queries = eqService.GetQueryList(modelId);
            return Json(queries);
        }

        /// <summary>
        /// This action is called when it's necessary to synchronize query on client side with its server-side copy.
        /// Additionally this action can be used to return a generated SQL statement (or several statements) as JSON string
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns><see cref="IActionResult"/> object</returns>
        [HttpPost]
        public IActionResult SyncQuery([FromBody] JsonDict jsonDict) {
            var queryDict = jsonDict["query"];
            var optionsDict = jsonDict["options"] as JsonDict;
            var query = eqService.SyncQueryDict(queryDict as JsonDict);
            var statement = eqService.BuildQuery(query, optionsDict);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("statement", statement);
            return Json(dict);
        }


        /// <summary>
        /// This action returns a custom list by different list request options (list name).
        /// </summary>
        /// <param name="jsonDict">GetList request options.</param>
        /// <returns><see cref="IActionResult"/> object</returns>
        [HttpPost]
        public IActionResult GetList([FromBody] JsonDict jsonDict) {
            return Json(eqService.GetList(jsonDict));
        }


        /// <summary>
        /// Executes the query and some options passed as JSON string and returns the result record set (again as JSON).
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns><see cref="IActionResult"/> object</returns>
        [HttpPost]
        public IActionResult ExecuteQuery([FromBody] JsonDict jsonDict) {
            var queryDict = jsonDict["query"] as JsonDict;
            var optionsDict = jsonDict["options"] as JsonDict;

            var query = eqService.GetQueryByJsonDict(queryDict);
            var sql = eqService.BuildQuery(query, optionsDict);

            var resultSet = eqService.ExecuteQuery(query, optionsDict);

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("statement", sql);
            dict.Add("resultSet", resultSet);
            dict.Add("resultCount", resultSet.RecordCount + " record(s) found");

            return Json(dict);
        }

        
        /// <summary>
        /// Returns XML file which represents the query passed in parameter as JSON string
        /// </summary>
        /// <param name="queryJson">JSON representation of the query</param>
        /// <returns><see cref="FileStreamResult"/> object which contains the query XML file</returns>
        [HttpPost]
        public FileStreamResult SaveQueryToFile(string queryJson) {
            var query = eqService.GetQueryByJsonDict(queryJson.ToJsonDict());
            MemoryStream stream = new MemoryStream();
            query.SaveToStream(stream);
            stream.Position = 0;
            return new FileStreamResult(stream, new MediaTypeHeaderValue("text/xml")) {
                FileDownloadName = "CurrentQuery.xml"
            };

        }


        /// <summary>
        /// Loads query XML uploaded from the client and saves it 
        /// </summary>
        /// <param name="queryId"></param>
        /// <param name="modelId"></param>
        /// <param name="queryFile"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LoadQueryFromFile(string modelId, IFormFile queryFile) {
            if (queryFile != null && queryFile.Length > 0)
                try {
                    var query = eqService.GetQuery(modelId, null);
                    using (var fileStream = queryFile.OpenReadStream()) {
                        query.LoadFromStream(fileStream);
                    }

                    //saves loaded query into session so it will be loaded automatically after redirect
                    eqService.SyncQuery(query);
                    return RedirectToAction("Index", new { queryId = query.ID });
                }
                catch {
                    //just do nothing  
                }
            else {

            }

            return RedirectToAction("Index");
        }




        #endregion

    }
}

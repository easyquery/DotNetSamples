using System;
using System.Collections.Generic;
using System.Linq;

using System.Data.SqlClient;
using System.Data;

using System.IO;

using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.Db;

using Korzh.Utils;

using Korzh.EasyQuery.AspNetCore.Demo03.Session;

namespace Korzh.EasyQuery.AspNetCore.Demo03
{

    public class EasyReportController : Controller {
        /*
            static EasyReportController()
            {
                //The following section adds support for different types of databases you may use in your app.
                //It's necessary for proper work of LoadFromConnection method of DbModel class
                //Please note: for each used DbGate class you will need to reference a corresponding DbGate assembly
                  Korzh.EasyQuery.DataGates.OleDbGate.Register();
                  Korzh.EasyQuery.DataGates.SqlClientGate.Register();
                  Korzh.EasyQuery.SqlCEDBGate.SqlCeGate.Register();
            }
            */
        private EqServiceProviderDb eqService;

        public EasyReportController(IHostingEnvironment env, IConfiguration config) {
            eqService = new EqServiceProviderDb();

            eqService.Paging.Enabled = true;
            eqService.DefaultModelId = "NWindSQL";
            eqService.UserId = "TestUser";
            eqService.StoreModelInCache = true;
            eqService.StoreQueryInCache = true;


            //EqServiceProvider needs to know where to save/load queries to/from

            eqService.Formats.SetDefaultFormats(FormatType.MsSqlServer);
            eqService.Formats.UseSchema = true;

            string dataPath = System.IO.Path.Combine(env.ContentRootPath, "App_Data");
            eqService.DataPath = dataPath;

            eqService.ConnectionResolver = () => {
                return new SqlConnection(config.GetConnectionString("EqDemoDb"));
            };


            //You can set DbConnection directly (without using ConfigurationManager)
            //eqService.Connection = new SqlCeConnection("Data Source=" + System.IO.Path.Combine(dataPath, "Northwind.sdf"));

            //to support saving/loading models and queries to/from Session 
            eqService.CacheGetter = (key) => HttpContext.Session.GetString(key);
            eqService.CacheSetter = (key, value) => HttpContext.Session.SetString(key, value.ToString());


            //The following four handlers (QuerySaver, QueryLoader, QueryRemover and QueryListResolver) are overrided in order to don't save to the server the changes user make - all changed/added queries are stored in Session object 
            //This is for demo purpose only, you may freely delete this code or modify to your notice
            // --- begining of overrided handlers ---

            eqService.QuerySaver = (query, queryId) => {
                if (!string.IsNullOrEmpty(queryId)){
                    HttpContext.Session.SetString("query" + queryId, query.SaveToString());

                    List<QueryListItem> queries = HttpContext.Session.GetObject<List<QueryListItem>>("queryList");
                    if (queries != null){
                        QueryListItem item = queries.Find(x => x.id.Equals(queryId));
                        if (item == null){
                            item = new QueryListItem(query.ID, query.QueryName);
                            queries.Add(item);
                        }
                        item.name = query.QueryName;
                        item.description = query.QueryDescription;

                        HttpContext.Session.SetObject("queryList", queries);
                    }

                }
            };

            eqService.QueryLoader = (query, queryId) => {
                if (!string.IsNullOrEmpty(queryId)){
                    string queryString = HttpContext.Session.GetString("query" + queryId);
                    if (String.IsNullOrWhiteSpace(queryString)){
                        eqService.DefaultQueryLoader(query, queryId);
                        HttpContext.Session.SetString("query" + queryId, query.SaveToString());
                    }
                    else
                        query.LoadFromString(queryString);
                }
            };

            eqService.QueryRemover = (queryId) => {
                if (!string.IsNullOrEmpty(queryId)){
                    HttpContext.Session.Remove("query" + queryId);

                    List<QueryListItem> queries = HttpContext.Session.GetObject<List<QueryListItem>>("queryList");
                    if (queries != null){
                        QueryListItem item = queries.Find(x => x.id.Equals(queryId));
                        if (item != null) queries.Remove(item);
                    }
                }
            };
            
            eqService.QueryListResolver = (modelId) => {
                List<QueryListItem> queryItems = HttpContext.Session.GetObject<List<QueryListItem>>("queryList");

                if (queryItems == null) {
                    queryItems = eqService.DefaultQueryListResolver(modelId) as List<QueryListItem>;

                    HttpContext.Session.SetObject("queryList", queryItems);
                }

                return queryItems;
            }; 
            
            // --- end of overrided handlers ---


            //Uncomment in case you need to implement your own model loader or add some changes to existing one
            // eqService.ModelLoader = (model, modelName) => {
            //   model.LoadFromConnection(eqService.Connection, FillModelOptions.Default);
            //   model.
            // };

            //Custom lists resolver
            eqService.ValueListResolver = (listname) => {
                if (listname == "ListName") {
                    return new List<ListItem> {
                            new ListItem("ID1","Item 1"),
                            new ListItem("ID2","Item 2")
                        };
                }
                return Enumerable.Empty<ListItem>();
            };

        }
        /*
            protected void OnException(ExceptionContext filterContext)
            {
                if (filterContext.ExceptionHandled)
                {
                    return;
                }
                filterContext.Result = new JsonResult("Error: " + filterContext.Exception.Message);

                filterContext.ExceptionHandled = true;
            }
            */

        #region public actions

        /// <summary>
        /// Gets the model by its modelId
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns><see cref="IActionResult"/> object</returns>
        [HttpPost]
        public IActionResult GetModel([FromBody] JsonDict jsonDict) {
            string modelId = jsonDict["modelId"].ToString();
            var model = eqService.GetModel(modelId);
            return Json(model.SaveToJsonDict());
        }

        /// <summary>
        /// This action returns a custom list by different list request options (list name).
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns><see cref="IActionResult"/> object</returns>
        [HttpPost]
        public IActionResult GetList([FromBody] JsonDict options) {
            return Json(eqService.GetList(options));
        }

        /// <summary>
        /// Gets the query by its JsonDict
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns><see cref="IActionResult"/> object</returns>
        [HttpPost]
        public IActionResult GetQuery([FromBody] JsonDict jsonDict){
            var query = eqService.GetQueryByJsonDict(jsonDict);
            return Json(query.SaveToJsonDict());
        }

        /// <summary>
        /// Gets the list of saved queries 
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns>IActionResult object</returns>
        [HttpPost]
        public JsonResult GetQueryList([FromBody] JsonDict jsonDict){
            string modelId = jsonDict["modelId"].ToString();
            var queries = eqService.GetQueryList(modelId);

            return Json(queries);
        }

        /// <summary>
        /// Creates new query.
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns><see cref="IActionResult"/> object</returns>
        [HttpPost]
        public IActionResult NewQuery([FromBody] JsonDict jsonDict) {
            string queryName = jsonDict["queryName"].ToString();
            JsonDict Temp = new JsonDict();
            var query = eqService.SaveQueryDict(Temp, queryName);

            return Json(query.SaveToJsonDict());
        }

        /// <summary>
        /// Saves the query.
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns><see cref="IActionResult"/> object</returns>
        [HttpPost]
        public IActionResult SaveQuery([FromBody] JsonDict jsonDict){
            var queryDict = jsonDict["query"] as JsonDict;
            string queryName = jsonDict["queryName"].ToString();

            var query = eqService.SaveQueryDict(queryDict, queryName);

            //we return a JSON object with one property "query" that contains the definition of saved query
            var dict = new Dictionary<string, object>();
            dict.Add("query", query.SaveToJsonDict());
            return Json(dict);
        }

        /// <summary>
        /// Removes the query
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns><see cref="IActionResult"/> object</returns>
        [HttpPost]
        public IActionResult RemoveQuery([FromBody] JsonDict jsonDict) {
            if (!jsonDict.TryGetValue("id", out object obj)) {
                obj = jsonDict["queryId"];
            }
            var queryId = obj.ToString();

            eqService.RemoveQuery(queryId);

            var dict = new Dictionary<string, object>();
            dict.Add("result", "ok");

            return Json(dict);
        }

        /// <summary>
        /// It's called when it's necessary to synchronize query on client side with its server-side copy.
        /// Additionally this action can be used to return a generated SQL statement (or several statements) as JSON string
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns><see cref="IActionResult"/> object</returns>
        [HttpPost]
        public IActionResult SyncQuery([FromBody] JsonDict jsonDict) {
            var queryDict = jsonDict["query"];
            var optionsDict = jsonDict["options"] as JsonDict;

            var query = eqService.SyncQueryDict(queryDict as JsonDict);
            //var qbr = eqService.BuildQuery(query, optionsDict);

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("result", "ok");
            return Json(dict);
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

            eqService.LoadOptions(optionsDict);                

            var query = eqService.GetQueryByJsonDict(queryDict);
            var qbr = eqService.BuildQuery(query, optionsDict);

            var resultSet = eqService.ExecuteQuery(query, optionsDict);

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("statement", qbr.Statement);
            dict.Add("resultSet", resultSet);
            dict.Add("paging", eqService.Paging.SaveToJsonDict());
            return Json(dict);
        }

        public void ErrorResponse(string msg) {
            Response.StatusCode = 400;
            Response.WriteAsync(msg);
        }

        /// <summary>
        /// Writes information to CSV file.
        /// </summary>
        /// <param name="queryJson">The string query which contains request parameters</param>
        /// <param name="fileType">type of the file</param>
        /// <returns></returns>
        [HttpPost]
        public void ExportToFile(string queryJson, string fileType){
            
            var query = eqService.GetQueryByJsonDict(queryJson.ToJsonDict());
            var qbr = eqService.BuildQuery(query);

            switch (fileType) {
                case "csv":
                    ExportToFileCsv(qbr.Statement);
                    break;
                case "excel/html":
                    ExportToFileExcel(qbr.Statement);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Writes information to Excel.Html file.
        /// </summary>
        /// <returns></returns>
        private void ExportToFileExcel(string sql) {
            HttpContext.Response.Clear();
            
            if (!string.IsNullOrEmpty(sql)){


                using (var connection = eqService.ConnectionResolver()){
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;

                    var dataset = command.ExecuteReader();

                    if (dataset != null){
                        HttpContext.Response.ContentType = "application/vnd.ms-excel";
                        HttpContext.Response.Headers.Add("Content-Disposition",
                            string.Format("attachment; filename=\"{0}\"", UrlEncoder.Default.Encode("report.xls")));
                        StreamWriter dataStream = new StreamWriter(HttpContext.Response.Body);
                        DbExport.ExportToExcelHtml(dataset, dataStream, HtmlFormats.Default);
                    }
                    else
                        ErrorResponse("Empty dataset");

                    eqService.ConnectionResolver().Close();
                }

            }
            else
                ErrorResponse("Empty query");
        }

        /// <summary>
        /// Writes information to CSV file.
        /// </summary>
        /// <returns></returns>
        private void ExportToFileCsv(string sql) {
            HttpContext.Response.Clear();

            if (!string.IsNullOrEmpty(sql)){


                using (var connection = eqService.ConnectionResolver()){
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;

                    var dataset = command.ExecuteReader();

                    if (dataset != null){
                        HttpContext.Response.ContentType = "text/csv"; ;
                        HttpContext.Response.Headers.Add("Content-Disposition",
                            string.Format("attachment; filename=\"{0}\"", UrlEncoder.Default.Encode("report.csv")));
                        StreamWriter dataStream = new StreamWriter(HttpContext.Response.Body);
                        DbExport.ExportToCsv(dataset, dataStream, CsvFormats.Default);
                    }
                    else
                        ErrorResponse("Empty dataset");

                    eqService.ConnectionResolver().Close();
                }

            }
            else
                ErrorResponse("Empty query");
        }



        #endregion


    }

}

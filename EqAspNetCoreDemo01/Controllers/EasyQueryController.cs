using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.IO;
using System.Text.Encodings.Web;

using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;


using Microsoft.AspNetCore.Mvc;

using System.Data.SqlClient;
using System.Data;

using Korzh.EasyQuery.Db;
using Korzh.EasyQuery.Services;

using Korzh.Utils;

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

            eqService.Paging.Enabled = true;

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
                if (listname.StartsWith("Cities")) {

                    var connection = eqService.ConnectionResolver() as SqlConnection;
                    var command = connection.CreateCommand();

                    string entityCity = eqService.Model.EntityRoot.FindAttribute(EntityAttrProp.Expression, "Customers.City").Expr;
                    string entityCountry = eqService.Model.EntityRoot.FindAttribute(EntityAttrProp.Expression, "Customers.Country").Expr;

                    if (listname == "Cities.") {
                        command.CommandText = string.Format("SELECT DISTINCT {0} FROM Customers", entityCity);
                    }
                    else {
                        var param = listname.Substring(listname.IndexOf('.') + 1);
                        command.CommandText = string.Format("SELECT DISTINCT {0} FROM Customers WHERE {1} IN ({2})", entityCity, entityCountry, AddQuotesForParams(param));
                    }

                    try {

                        connection.Open();
                        var dataReader = command.ExecuteReader();

                        var valueList = new List<ListItem>();
                        while (dataReader.Read()) {
                            valueList.Add(new ListItem(dataReader[entityCity].ToString()));
                        }

                        dataReader.Close();
                        connection.Close();

                        return valueList;
                    }
                    finally {
                        connection.Close();
                    }

                }
                return Enumerable.Empty<ListItem>();
            };

        }

        private string AddQuotesForParams(string param) {
            string[] Params = param.Split(',');
            string result = "";
            foreach (var p in Params) {
                result += "'" + p + "',";
            }
            result = result.Remove(result.Length - 1);
            return result;
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
            var qbr = eqService.BuildQuery(query, optionsDict);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("statement", qbr.Statement);
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

            eqService.LoadOptions(optionsDict);

            var query = eqService.GetQueryByJsonDict(queryDict);
            var qbr = eqService.BuildQuery(query, optionsDict);

            var resultSet = eqService.ExecuteQuery(query, optionsDict);

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("statement", qbr.Statement);
            dict.Add("resultSet", resultSet);
            dict.Add("paging", eqService.Paging.SaveToJsonDict());
            dict.Add("resultCount", ((eqService.Paging.Enabled) ? eqService.Paging.TotalRecords : resultSet.RecordCount) + " record(s) found");

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

        public void ErrorResponse(string msg){
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

            switch (fileType){
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
        private void ExportToFileExcel(string sql){
            HttpContext.Response.Clear();

            if (!string.IsNullOrEmpty(sql)){


                using (var connection = eqService.ConnectionResolver()) {
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
        private void ExportToFileCsv(string sql){
            HttpContext.Response.Clear();

            if (!string.IsNullOrEmpty(sql)){


                using (var connection = eqService.ConnectionResolver()){
                    connection.Open();
                    var command = connection.CreateCommand();

                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;

                    var dataset = command.ExecuteReader();

                    if (dataset != null) {
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

using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Text;

using Microsoft.AspNetCore.Mvc;

using System.Data.SqlClient;

using Korzh.EasyQuery.Db;
using Korzh.EasyQuery.Services;

using Korzh.EasyQuery.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace Korzh.EasyQuery.AspNetCore.Demo01
{

    public class EasyQueryController : Controller {

        private EqServiceProviderDb eqService;

        public EasyQueryController(IHostingEnvironment env, IConfiguration config) {
            eqService = new EqServiceProviderDb();
            eqService.DefaultModelName = "NWindSQL";            

            //eqService.SessionGetter = key => Session[key];
            //eqService.SessionSetter = (key, value) => Session[key] = value;

            eqService.StoreQueryInSession = false;

            eqService.Formats.SetDefaultFormats(FormatType.MsSqlServer);
            eqService.Formats.UseSchema = true;

            string dataPath = System.IO.Path.Combine(env.ContentRootPath , "App_Data");
            eqService.DataPath = dataPath;  

            eqService.Connection = new SqlConnection(config.GetConnectionString("EqDemoDb"));

            eqService.CustomListResolver = (listname) => {
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

        public ActionResult Index() {
            //var query = eqService.GetQuery();
            //ViewBag.QueryJson = query.SaveToDictionary().ToJson();
            //ViewBag.Message = TempData["Message"];
            return View("EasyQuery");
        }



        /// <summary>
        /// Creates a <see cref="T:System.Web.Mvc.JsonResult" /> object that serializes the specified object to JavaScript Object Notation (JSON) format using the content type, content encoding, and the JSON request behavior.
        /// </summary>
        /// <remarks>We override this method to set MaxJsonLength property to the maximum possible value</remarks>
        /// <param name="data">The JavaScript object graph to serialize.</param>
        /// <param name="contentType">The content type (MIME type).</param>
        /// <param name="contentEncoding">The content encoding.</param>
        /// <param name="behavior">The JSON request behavior</param>
        /// <returns>The result object that serializes the specified object to JSON format.</returns>
        //protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior) {
        //    return new JsonResult {
        //        Data = data,
        //        ContentType = contentType,
        //        ContentEncoding = contentEncoding,
        //        JsonRequestBehavior = behavior,
        //        MaxJsonLength = int.MaxValue
        //    };
        //}

        #region public actions
        /// <summary>
        /// Gets the model by its name
        /// </summary>
        /// <param name="modelName">The name.</param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult GetModel(string modelName) {
            var model = eqService.GetModel(modelName);
            return Json(model.SaveToDictionary());
        }

        /// <summary>
        /// Gets the query by its name
        /// </summary>
        /// <param name="queryName">The name.</param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult GetQuery(string queryId)
        {
            var query = eqService.GetQuery(queryId);
            return Json(query.SaveToDictionary());
        }

        /// <summary>
        /// Saves the query.
        /// </summary>
        /// <param name="queryJson">The JSON representation of the query .</param>
        /// <param name="queryName">Query name.</param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult SaveQuery(string queryJson, string queryName)
        {
            eqService.SaveQueryDict(queryJson.JsonToDictionary(), queryName);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("result", "OK");
            return Json(dict);
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public JsonResult GetQueryList(string modelName)
        {
            var queries = eqService.GetQueryList(modelName);
            return Json(queries);
        }

        /// <summary>
        /// It's called when it's necessary to synchronize query on client side with its server-side copy.
        /// Additionally this action can be used to return a generated SQL statement (or several statements) as JSON string
        /// </summary>
        /// <param name="queryJson">The JSON representation of the query .</param>
        /// <param name="optionsJson">The additional parameters which can be passed to this method to adjust query statement generation.</param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult SyncQuery(string queryJson, string optionsJson)
        {
            var query = eqService.SyncQueryDict(queryJson.JsonToDictionary());
            var statement = eqService.BuildQuery(query, optionsJson.JsonToDictionary());
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("statement", statement);
            return Json(dict);
        }

        /// <summary>
        /// This action returns a custom list by different list request options (list name).
        /// </summary>
        /// <param name="options">List request options - an instance of <see cref="ListRequestOptions"/> type.</param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult GetList(ListRequestOptions options)
        {
            return Json(eqService.GetList(options));
        }

        /// <summary>
        /// Executes the query passed as JSON string and returns the result record set (again as JSON).
        /// </summary>
        /// <param name="queryJson">The JSON representation of the query.</param>
        /// <param name="optionsJson">Different options in JSON format</param>
        /// <returns></returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult ExecuteQuery(string queryJson, string optionsJson) {

            var query = eqService.LoadQueryDict(queryJson.JsonToDictionary());
            var queryOptions = optionsJson.JsonToDictionary();
            var sql = eqService.BuildQuery(query, queryOptions);

            var resultSet = eqService.ExecuteQuery(query, queryOptions);

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("statement", sql);
            dict.Add("resultSet", resultSet);
            dict.Add("resultCount", resultSet.RecordCount + " record(s) found");


            return Json(dict);
        }


        private void ErrorResponse(string msg) {
            Response.StatusCode = 400;
            Response.WriteAsync(msg);
        }


        //[HttpGet]
        //public void ExportToFileExcel() {
        //    Response.Clear();

        //    var query = eqService.GetQuery();

        //    if (!query.IsEmpty) {
        //        var sql = eqService.BuildQuery(query);
        //        eqService.Paging.Enabled = false;
        //        DataSet dataset = eqService.GetDataSetBySql(sql);
        //        if (dataset != null) {
        //            Response.ContentType = "application/vnd.ms-excel";
        //            Response.AddHeader("Content-Disposition",
        //                string.Format("attachment; filename=\"{0}\"", HttpUtility.UrlEncode("report.xls")));
        //            DbExport.ExportToExcelHtml(dataset, Response.Output, HtmlFormats.Default);
        //        }
        //        else
        //            ErrorResponse("Empty dataset");
        //    }
        //    else
        //        ErrorResponse("Empty query");
            
        //}

        //[HttpGet]
        //public void ExportToFileCsv() {
        //    Response.Clear();
        //    var query = eqService.GetQuery();

        //    if (!query.IsEmpty) {
        //        var sql = eqService.BuildQuery(query);
        //        eqService.Paging.Enabled = false;
        //        DataSet dataset = eqService.GetDataSetBySql(sql);
        //        if (dataset != null) {
        //            Response.ContentType = "text/csv";
        //            Response.AddHeader("Content-Disposition",
        //                string.Format("attachment; filename=\"{0}\"", HttpUtility.UrlEncode("report.csv")));
        //            DbExport.ExportToCsv(dataset, Response.Output, CsvFormats.Default);
        //        }
        //        else
        //            ErrorResponse("Empty dataset");
        //    }
        //    else
        //        ErrorResponse("Empty query");

        //}

        [HttpGet]
        public FileResult GetCurrentQuery() {
            var query = eqService.GetQuery();
            FileContentResult result = new FileContentResult(Encoding.UTF8.GetBytes(query.SaveToString()), "Content-disposition: attachment;");
            result.FileDownloadName = "CurrentQuery.xml";
            return result;
        }

        [HttpPost]
        public IActionResult LoadQueryFromFile(IFormFile queryFile) {  
            if (queryFile != null && queryFile.Length > 0)  
                try {
                    var query = eqService.GetQuery();
                    using (var fileStream = queryFile.OpenReadStream()) {
                        query.LoadFromStream(fileStream);
                    }
                    eqService.SyncQuery(query);
                }  
                catch (Exception ex){  
                    TempData["Message"] = "ERROR:" + ex.Message.ToString();  
                }  
            else{  
                TempData["Message"] = "You have not specified a file.";  
            }

            return RedirectToAction("Index");
        }


        #endregion

    }
}

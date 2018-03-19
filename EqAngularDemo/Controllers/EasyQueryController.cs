using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Data;
using System.IO;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

using Microsoft.EntityFrameworkCore;

using System.Text.Encodings.Web;

using Korzh.EasyQuery;
using Korzh.EasyQuery.Db;
using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.EntityFrameworkCore;

using Korzh.Utils;


using EqAngularDemo.Data;

namespace EqAngularDemo.Controllers
{

    public class EasyQueryController : Controller {

        private EqServiceProviderDb eqService;
        private AppDbContext _dbContext;

        public EasyQueryController(AppDbContext dbContext, IHostingEnvironment env) {
            this._dbContext = dbContext;

            eqService = new EqServiceProviderDb();

            eqService.StoreQueryInCache = false;

            string dataPath = System.IO.Path.Combine(env.ContentRootPath, "App_Data");
            eqService.DataPath = dataPath;

            eqService.Formats.SetDefaultFormats(FormatType.MsSqlServer);
            eqService.Formats.UseSchema = true;

            eqService.ModelLoader = (model, modelName) => {
                (model as DbModel).LoadFromDbContext(dbContext);
            };


            eqService.ConnectionResolver = () => {
                return dbContext.Database.GetDbConnection();
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
        /// Saves the query passed in request with new name
        /// </summary>
        /// <param name="jsonDict">The JsonDict object which contains request parameters</param>
        /// <returns><see cref="IActionResult"/> object</returns>
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
            var queryDict = jsonDict["query"] as JsonDict;
            var optionsDict = jsonDict["options"] as JsonDict;
            var query = eqService.SyncQueryDict(queryDict);
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

            var query = eqService.GetQueryByJsonDict(queryDict);
            var qbr = eqService.BuildQuery(query, optionsDict);

            var resultSet = eqService.ExecuteQuery(query, optionsDict);

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("statement", qbr.Statement);
            dict.Add("resultSet", resultSet);
            dict.Add("resultCount", ((eqService.Paging.Enabled) ? eqService.Paging.TotalRecords : resultSet.RecordCount) + " record(s) found");

            return Json(dict);
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
        private void ExportToFileExcel(string sql)
        {
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

                    if (dataset != null)
                    {
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


    }
}

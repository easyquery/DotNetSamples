using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;


using Korzh.EasyQuery.Db;
using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.EntityFrameworkCore;
using Korzh.EasyQuery.AspNetCore;


using Korzh.EasyQuery.AspNetCore.Demo02.Data;
using Microsoft.AspNetCore.Hosting;

namespace Korzh.EasyQuery.AspNetCore.Demo02.Controllers
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

        public ActionResult Index() {

            return View("EasyQuery");
        }



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
        public ActionResult GetQuery(string queryId) {
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
        public ActionResult SaveQuery(string queryJson, string queryName) {
            eqService.SaveQueryDict(queryJson.JsonToDictionary(), queryName);
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("result", "OK");
            return Json(dict);
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public JsonResult GetQueryList(string modelName) {
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
        public ActionResult SyncQuery(string queryJson, string optionsJson) {
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
        public ActionResult GetList(ListRequestOptions options) {
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

            var customers = _dbContext.Customers.FromSql(sql);

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


        #endregion

    }
}

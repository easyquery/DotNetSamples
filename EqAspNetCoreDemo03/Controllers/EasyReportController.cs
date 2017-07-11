using System;
using System.Collections.Generic;
using System.Linq;

using System.Data.SqlClient;
using System.IO;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.Db;


using Korzh.EasyQuery.AspNetCore.Demo03.Session;

namespace Korzh.EasyQuery.AspNetCore.Demo03
{
    public class EasyReportController : Controller
    {
        /*
            static EasyReportController()
            {
                //The following section adds support for different types of databases you may use in your app.
                //It's necessary for proper work of LoadFromConnection method of DbModel class
                //Please note: for each used DbGate class you will need to reference a corresponding DbGate assembly
            //    Korzh.EasyQuery.DataGates.OleDbGate.Register();
             //   Korzh.EasyQuery.DataGates.SqlClientGate.Register();
                //Korzh.EasyQuery.SqlCEDBGate.SqlCeGate.Register();
            }
            */
        private EqServiceProviderDb eqService;

        public EasyReportController(IHostingEnvironment env, IConfiguration config)
        {
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
                if (!string.IsNullOrEmpty(queryId)) {
                    HttpContext.Session.SetString("query" + queryId, query.SaveToString());

                    List<QueryListItem> queries = HttpContext.Session.GetObject<List<QueryListItem>>("queryList");
                    if (queries != null)
                    {
                        QueryListItem item = queries.Find(x => x.id.Equals(queryId));
                        if (item == null) {
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
                if (!string.IsNullOrEmpty(queryId))
                {
                    string queryString = HttpContext.Session.GetString("query" + queryId);
                    if (String.IsNullOrWhiteSpace(queryString)) {
                        eqService.DefaultQueryLoader(query, queryId);
                        HttpContext.Session.SetString("query" + queryId, query.SaveToString());
                    }
                    else
                        query.LoadFromString(queryString);
                }
            };

            eqService.QueryRemover = (queryId) => {
                if (!string.IsNullOrEmpty(queryId)) {
                    HttpContext.Session.Remove("query" + queryId);

                    List<QueryListItem> queries = HttpContext.Session.GetObject<List<QueryListItem>>("queryList");
                    if (queries != null) {
                        QueryListItem item = queries.Find(x => x.id.Equals(queryId));
                        if (item != null) queries.Remove(item);
                    }
                }
            };

            //eqService.QueryListResolver = (modelId) => {
            //    List<QueryListItem> queryItems = HttpContext.Session.GetObject<List<QueryListItem>>("queryList");

            //    if (queryItems == null) {
            //        queryItems = eqService.DefaultQueryListResolver(modelId) as List<QueryListItem>;

            //        HttpContext.Session.SetObject("queryList", queryItems);
            //    }

            //    return queryItems;
            //};

            // --- end of overrided handlers ---


            //Uncomment in case you need to implement your own model loader or add some changes to existing one
            // eqService.ModelLoader = (model, modelId) => {
            //   model.LoadFromConnection(eqService.Connection, FillModelOptions.Default);
            // };

            //Custom lists resolver
            eqService.ValueListResolver = (listname) => {
                if (listname == "ListName")
                {
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
        /// Gets the model by its name
        /// </summary>
        /// <param name="modelName">The name.</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetModel([FromBody] JsonDict jsonDict)
        {
            string modelId = jsonDict["modelId"].ToString();
            var model = eqService.GetModel(modelId);
            return Json(model.SaveToJsonDict());
        }


        /// <summary>
        /// This action returns a custom list by different list request options (list name).
        /// </summary>
        /// <param name="options">List request options - an instance of <see cref="ListRequestOptions"/> type.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetList(JsonDict jsonDict)
        {
            //Точно не разобрался, как тут 
            return Json(jsonDict.ToListViewOptions());
        }

        /// <summary>
        /// Gets the query by its name
        /// </summary>
        /// <param name="queryName">The name.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetQuery(string modelId, string queryId)
        {
            var query = eqService.GetQuery(modelId, queryId);
            return Json(query.SaveToJsonDict());
        }


        [HttpPost]
        public JsonResult GetQueryList(string modelId)
        {
            try
            {
                var queries = eqService.GetQueryList(modelId);

                return Json(queries);
            }
            catch (Exception ex)
            {
                return Json(ex.Message + "\n" + ex.StackTrace);
            }
        }



        [HttpPost]
        public ActionResult NewQuery(string modelId, string queryName)
        {
            JsonDict Temp = new JsonDict();
            var query = eqService.SaveQueryDict(Temp, queryName);

            return Json(query.SaveToJsonDict());
        }


        /// <summary>
        /// Saves the query.
        /// </summary>
        /// <param name="queryJson">The JSON representation of the query .</param>
        /// <param name="queryName">Query name.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveQuery(string queryJson, string queryName)
        {
            var query = eqService.SaveQueryDict(queryJson.ToJsonDict(), queryName);

            //we return a JSON object with one property "query" that contains the definition of saved query
            var dict = new Dictionary<string, object>();
            dict.Add("query", query.SaveToJsonDict());
            return Json(dict);
        }


        [HttpPost]
        public ActionResult RemoveQuery(string queryId)
        {
            eqService.RemoveQuery(queryId);
            var dict = new Dictionary<string, object>();
            dict.Add("result", "ok");
            return Json(dict);
        }


        /// <summary>
        /// It's called when it's necessary to synchronize query on client side with its server-side copy.
        /// Additionally this action can be used to return a generated SQL statement (or several statements) as JSON string
        /// </summary>
        /// <param name="queryJson">The JSON representation of the query .</param>
        /// <param name="optionsJson">The additional parameters which can be passed to this method to adjust query statement generation.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SyncQuery(string queryJson, string optionsJson)
        {
            JsonDict queryDict = queryJson.ToJsonDict();
            var query = eqService.GetQueryByJsonDict(queryDict);

            eqService.SyncQuery(query);
            if (!string.IsNullOrEmpty(query.ID))
                eqService.SaveQuery(query);

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("result", "ok");
            return Json(dict);
        }


        /// <summary>
        /// Executes the query passed as JSON string and returns the result record set (again as JSON).
        /// </summary>
        /// <param name="queryJson">The JSON representation of the query.</param>
        /// <param name="optionsJson">Different options in JSON format</param>
        /// <returns></returns>
        
        [HttpPost]
        public ActionResult ExecuteQuery(string queryJson, string optionsJson)
        {
            var optionsDict = optionsJson.ToJsonDict();

            var query = eqService.GetQueryByJsonDict(queryJson.ToJsonDict());
            var statement = eqService.BuildQuery(query, optionsDict);
            var resultSet = eqService.GetResultSetBySql(statement);

            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("statement", statement);
            dict.Add("resultSet", resultSet);
            dict.Add("paging", eqService.Paging.SaveToJsonDict());

            return Json(dict);
        }
        
       

        public void ErrorResponse(string msg)
        {
            Response.StatusCode = 400;
            Response.WriteAsync(msg);
        }



        [HttpGet]
        public void ExportToFileExcel()
        {


        }

        [HttpGet]
        public void ExportToFileCsv()
        {


        }
        #endregion


    }

}

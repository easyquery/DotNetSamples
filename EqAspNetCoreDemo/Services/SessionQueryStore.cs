using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using Korzh.EasyQuery;
using Korzh.EasyQuery.Services;

namespace EqAspNetCoreDemo.Services
{
    public class SessionQueryStore : IQueryStore
    {
        private const string _keyPrefixQuery = "query-";
        private const string _keyPrefixItem = "items-";

        private HttpContext _httpContext;

        protected readonly IServiceProvider Services;

        private string fileStoreDataPath;

        public SessionQueryStore(IServiceProvider services, string dataPath = "App_Data")
        {
            fileStoreDataPath = dataPath;
            Services = services;
            _httpContext = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext 
                           ?? throw new ArgumentNullException("IHttpContextAccessor or HttpContext is null.");
        }

        public async Task<bool> AddQueryAsync(Query query)
        {
            _httpContext.Session.SetString(_keyPrefixQuery + query.ID, await query.SaveToJsonStringAsync());
            AddQueryListItem(query.Model.ID, new QueryListItem(query.ID, query.Name));
            return true;
        }

        public Task<IEnumerable<QueryListItem>> GetAllQueriesAsync(string modelId)
        {
            return Task.FromResult(GetQueryListItems(modelId).OrderBy(item => item.name).AsEnumerable());
        }

        public async Task<bool> LoadQueryAsync(Query query, string queryId)
        {
            var queryJson = _httpContext.Session.GetString(_keyPrefixQuery + queryId);
            if (!string.IsNullOrEmpty(queryJson)) {
                await query.LoadFromJsonStringAsync(queryJson);
                return true;
            }

            return false;
        }

        public Task<bool> RemoveQueryAsync(string modelId, string queryId)
        {
            _httpContext.Session.Remove(_keyPrefixQuery + queryId);
            RemoveQueryListItem(modelId, queryId);
            return Task.FromResult(true);
        }

        public async Task<bool> SaveQueryAsync(Query query, bool createIfNotExists = true)
        {
            var queryJson = _httpContext.Session.GetString(_keyPrefixQuery + query.ID);
            if (!string.IsNullOrEmpty(queryJson)) {

                _httpContext.Session.SetString(_keyPrefixQuery + query.ID, await query.SaveToJsonStringAsync());
                UpdateQueryListItem(query.Model.ID, new QueryListItem(query.ID, query.Name));
                return true;
            }
            else if (createIfNotExists) {
                return await AddQueryAsync(query);
            }

            return false;
        }

        private List<QueryListItem> GetQueryListItems(string modelId)
        {
            var json = _httpContext.Session.GetString(_keyPrefixItem + modelId);
            if (json == null) {
                FileQueryStore initialQueryStore = new FileQueryStore(fileStoreDataPath);

                List<QueryListItem> initialQueryList = initialQueryStore.GetAllQueriesAsync(modelId).Result.ToList();

                initialQueryList.ForEach(delegate (QueryListItem item) {
                    //_httpContext.Session.SetString(_keyPrefixQuery + item.id, initialQueryStore.GetQueryFileText(initialModelId, item.id));
                });

                SaveQueryListItems(modelId, initialQueryList);

                return initialQueryList;
            }

            var queryItemObj = new
            {
                id = "",
                name = "",
                desacription = ""
            };

            List<T> CreateList<T>(T type)
            {
                return new List<T>();
            }

            var queryItems = CreateList(queryItemObj);

            var result = JsonConvert.DeserializeAnonymousType(json, queryItems);
            return result.Select(item => new QueryListItem(item.id, item.name)).ToList();
        }


        private void SaveQueryListItems(string modelId, List<QueryListItem> items)
        {
            _httpContext.Session.SetString(_keyPrefixItem + modelId, 
                JsonConvert.SerializeObject(items, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }

        private void AddQueryListItem(string modelId, QueryListItem item)
        {
            var items = GetQueryListItems(modelId);
            items.Add(item);
            SaveQueryListItems(modelId, items);
        }

        private void UpdateQueryListItem(string modelId, QueryListItem item)
        {
            var items = GetQueryListItems(modelId);
            foreach (var it in items) {
                if (it.id == item.id) {
                    it.name = item.name;
                    it.description = item.description;
                    break;
                }
            }
            SaveQueryListItems(modelId, items);

        }

        private void RemoveQueryListItem(string modelId, string itemId)
        {
            var items = GetQueryListItems(modelId);
            items.Remove(items.FirstOrDefault(item => item.id == item.id));
            SaveQueryListItems(modelId, items);
        }
    }
}

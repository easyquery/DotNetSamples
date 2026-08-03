using EasyData;
using Korzh.EasyQuery;
using Korzh.EasyQuery.Db;
using Korzh.EasyQuery.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EqDemo.Services;

public class EasyQueryConfigurator : IEasyQueryConfigurator
{
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public EasyQueryConfigurator(IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        _configuration = configuration;
        _logger = loggerFactory.CreateLogger("EasyQueryConfigurator");
    }

    public void Configure(EasyQueryOptions options)
    {
        options.UseManager<EasyQueryManagerSql>();
        options.DefaultModelId = "nwind";
        options.BuildQueryOnSync = true;
        options.SaveNewQuery = false;
        var dbConnectionString = _configuration.GetConnectionString("EqDemoDbLite");
        options.ConnectionString = dbConnectionString;
        options.UseDbContext<AppDbContext>();
        options.StoreModelInCache = true;
        options.StoreQueryInCache = true;

        //defining different query store depending on configuration
        if (string.Compare(_configuration.GetValue<string>("QueryStoreMode"), "session", true) == 0) {
            options.UseQueryStore(manager => new SessionQueryStore(manager.Services, "App_Data"));
        }
        else {
            options.UseQueryStore(_ => new FileQueryStore(new FileQueryStoreSettings {
                DataPath = "App_Data",
                FileFormat = "xml"
            }));
        }

        options.UseModelTuner(manager => {
            var attr = manager.Model.FindEntityAttr("Order.ShipRegion");
            attr.Operations.RemoveByIDs(manager.Model, "StartsWith,Contains");
            attr.DefaultEditor = new CustomListValueEditor("Lookup", "Lookup");

            var catNameAttr = manager.Model.FindEntityAttr("Category.CategoryName");
            var catIdAttr = manager.Model.FindEntityAttr("Product.Category");
            catIdAttr.Entity.Attributes.Add(catNameAttr);
            catIdAttr.UseInResult = false;
            catIdAttr.LookupAttr = catNameAttr;
        });

        //applies the full-text search text (sent by the client in the "data" section of the request)
        //to the query as a group of hidden (extra) conditions
        options.UseQueryTuner(manager => {
            var query = manager.Query;

            //The query object is kept in the session cache (StoreQueryInCache = true), so the conditions
            //added on the previous request must be dropped first - otherwise every new search would
            //stack another group of conditions on top of the previous ones.
            query.ExtraConditions.Conditions.Clear();

            var text = manager.ClientData.TryGetValue("text", out var textObj)
                ? textObj?.ToString()
                : null;

            if (string.IsNullOrWhiteSpace(text)) {
                return;
            }

            //The scope of the search is defined by the query itself: its result columns if it has any,
            //otherwise the entities used in its conditions. So we only need to say how to treat
            //the date/time fields here.
            var ftsOptions = new DbFullTextSearchOptions {
                //search in date/time fields as well (by their string representations)
                IncludeDateTimeFields = true
            };

            query.AddFullTextSearchConditions(text, ftsOptions);
        });

        options.UseSqlFormats(FormatType.Sqlite, formats => {
            formats.UseDbName = false;
            formats.UseSchema = false;
        });
    }
}
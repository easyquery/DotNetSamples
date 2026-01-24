using EasyData;
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
            catNameAttr.UseInConditions = false;
            catIdAttr.UseInResult = false;
            catIdAttr.LookupAttr = catNameAttr;
        });

        options.UseSqlFormats(FormatType.Sqlite, formats => {
            formats.UseDbName = false;
            formats.UseSchema = false;
        });
    }
}
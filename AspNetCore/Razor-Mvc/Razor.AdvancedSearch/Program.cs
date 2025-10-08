using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using EasyData.Export;
using Korzh.EasyQuery.Services;
using Korzh.EasyQuery.Db;

using EqDemo;
using EqDemo.Services;
using EasyData;

var builder = WebApplication.CreateBuilder(args);

// Only load env vars that begin with ADMIN_
builder.Configuration.AddEnvironmentVariables(prefix: "AdvancedSearch");

// Configuration
var configuration = builder.Configuration;
var dbConnectionString = configuration.GetConnectionString("EqDemoDbLite");

// EasyQuery static settings
Korzh.EasyQuery.RazorUI.Pages.AdvancedSearch.ExportFormats = new string[] { "pdf", "excel", "excel-html", "csv" };
// Korzh.EasyQuery.RazorUI.Pages.AdvancedSearch.ShowSqlPanel = true; // Uncomment to show SQL panel

// Services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(dbConnectionString)
    // options.UseSqlServer(dbConnectionString);
);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddEasyQuery()
    .UseSqlManager()
    .AddDefaultExporters()
    .AddDataExporter<PdfDataExporter>("pdf")
    .AddDataExporter<ExcelDataExporter>("excel")
    .UseSessionCache()
    .RegisterDbGate<Korzh.EasyQuery.DbGates.SqLiteGate>();
    // .RegisterDbGate<Korzh.EasyQuery.DbGates.SqlServerGate>();

builder.Services.AddRazorPages();

// To support non-Unicode code pages in PDF Exporter
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapEasyQuery(options => {
    options.DefaultModelId = "nwind";
    options.BuildQueryOnSync = true;
    options.SaveNewQuery = false;
    options.ConnectionString = dbConnectionString;
    options.UseDbContext<AppDbContext>();
    options.StoreModelInCache = true;
    options.StoreQueryInCache = true;

    options.UseQueryStore((_) => new FileQueryStore("App_Data"));

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
});

app.MapRazorPages();
app.MapControllers();

// Init demo database (if necessary)
app.EnsureDbInitialized(dbConnectionString, app.Environment);

app.Run();

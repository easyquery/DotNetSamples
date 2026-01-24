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

// Only load env vars that begin with AdvancedSearch_
builder.Configuration.AddEnvironmentVariables(prefix: "AdvancedSearch_");

// Configuration
var configuration = builder.Configuration;
var dbConnectionString = configuration.GetConnectionString("EqDemoDbLite");

// EasyQuery static settings
Korzh.EasyQuery.RazorUI.Pages.AdvancedSearch.ExportFormats = new string[] { "pdf", "excel", "excel-html", "csv" };
// Korzh.EasyQuery.RazorUI.Pages.AdvancedSearch.ShowSqlPanel = true; // Uncomment to show the SQL panel

// Services
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(dbConnectionString)
    // options.UseSqlServer(dbConnectionString);
);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddEasyQuery<EasyQueryConfigurator>()
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
    // all configurations are done in EasyQueryConfigurator
});

app.MapRazorPages();
app.MapControllers();

// Init demo database (if necessary)
app.EnsureDbInitialized(dbConnectionString, app.Environment);

app.Run();

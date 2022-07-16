using Microsoft.EntityFrameworkCore;

using EasyData.Export;
using Korzh.EasyQuery.Services;

using EqDemo;
using EqDemo.Data;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddDbContext<AppDbContext>(
    options => options.UseSqlite(builder.Configuration.GetConnectionString("EqDemoSqLite"))
    //options => options.UseSqlServer(builder.Configuration.GetConnectionString("EqDemoDb"))
);

services.AddDistributedMemoryCache();
services.AddSession();

services.AddEasyQuery()
        .UseSqlManager()
        .AddDefaultExporters()
        .AddDataExporter<PdfDataExporter>("pdf")
        .AddDataExporter<ExcelDataExporter>("excel")
        .RegisterDbGate<Korzh.EasyQuery.DbGates.SqLiteGate>();
        //.RegisterDbGate<Korzh.EasyQuery.DbGates.SqlServerGate>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

//to support non-Unicode code pages in PDF Exporter
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapEasyQuery(options => {
    options.DefaultModelId = "nwind";
    options.SaveNewQuery = false;
    options.BuildQueryOnSync = true;
    options.UseDbContext<AppDbContext>();

    // If you want to load model directly from DB metadata
    // remove (or comment) options.UseDbContext(...) call and uncomment the next 3 lines of code
    //options.ConnectionString = Configuration.GetConnectionString("EqDemoDb");
    //options.UseDbConnection<Microsoft.Data.SqlClient.SqlConnection>();
    //options.UseDbConnectionModelLoader();

    options.UseQueryStore((_) => new FileQueryStore("App_Data"));
});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

//Init demo database (if necessary)
app.EnsureDbInitialized(builder.Configuration, app.Environment);

app.Run();

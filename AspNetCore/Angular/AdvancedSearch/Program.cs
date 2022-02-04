using Microsoft.EntityFrameworkCore;

using EasyData;
using EasyData.Export;
using Korzh.EasyQuery.Services;

using EqDemo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("EqDemoSqLite"));
    //options.UseSqlServer(builder.Configuration.GetConnectionString("EqDemoDb"));
});

//Registering EasyQuery services
builder.Services.AddEasyQuery()
                .AddDefaultExporters()
                .AddDataExporter<PdfDataExporter>("pdf")
                .AddDataExporter<ExcelDataExporter>("excel")
                .UseSqlManager();
// Uncomment if you want to load model directly from DB               
// .RegisterDbGate<SqlServerGate>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAllPolicy",
        builder => {
            builder.AllowAnyOrigin();
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.WithExposedHeaders("Content-Disposition");
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllPolicy");
app.UseStaticFiles();
app.UseRouting();

//Adding EasyQuery API middleware
app.MapEasyQuery(options => {
    options.DefaultModelId = "nwind";

    options.SaveNewQuery = false;
    options.BuildQueryOnSync = true;

    options.UseDbContext<AppDbContext>();

    // Uncomment if you want to donwload model directly from DB
    // options.UseDbConnectionModelLoader();

    options.UseModelTuner(manager =>
    {
        // for onGetExpression example
        var attr = manager.Model.FindEntityAttr("Customer.CompanyName");
        if (attr != null) {
            attr.DefaultEditor = new TextValueEditor("ContactNameEditor");
        }
    });

    options.UseQueryStore((_) => new FileQueryStore("App_Data"));
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();

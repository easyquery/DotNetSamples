using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;

using EasyData.Export;
using Korzh.EasyQuery.Services;

using EqDemo;
using EqDemo.Models;
using EqDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("EqDemoDb"));
    //options.UseSqlServer(builder.Configuration.GetConnectionString("EqDemoDb"));
});


// Setting up authentication/authorization services
builder.Services.AddDefaultIdentity<IdentityUser>(opts => {
    //Password options
    opts.Password.RequiredLength = 4;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;
})
 .AddRoles<IdentityRole>()
 .AddDefaultUI()
 .AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<CookiePolicyOptions>(options => {
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => false;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.ConfigureApplicationCookie(options => {
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
});

builder.Services.AddCors(options => {
    options.AddPolicy(name: "AllowAllPolicy",
        builder => {
            builder.AllowAnyOrigin();
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.WithExposedHeaders("Content-Disposition");
        });
});

builder.Services.AddRazorPages();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Registering EasyQuery services
builder.Services.AddEasyQuery()
                .AddDefaultExporters()
                .AddDataExporter<PdfDataExporter>("pdf")
                .AddDataExporter<ExcelDataExporter>("excel")
                .UseSessionCache()
                .UseSqlManager();
// Uncomment if you want to load model directly from DB               
// .RegisterDbGate<SqLiteGate>();
// .RegisterDbGate<SqlServerGate>();

builder.Services.AddScoped<DefaultReportGenerator>();

var app = builder.Build();

//to support non-Unicode code pages in PDF Exporter
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
}
else {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Disable all Identity/Account/Manage actions
var redirectOptions = new RewriteOptions()
    .AddRedirect("(?i:identity/account/forgotpassword(/.*)?$)", "/")
    .AddRedirect("(?i:identity/account/manage(/.*)?$)", "/");
app.UseRewriter(redirectOptions);

app.UseHttpsRedirection();
app.UseCors("AllowAllPolicy");
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

//Adding EasyQuery API middleware
app.MapEasyQuery(options => {
    options.DefaultModelId = "adhoc-reporting";
    options.SaveNewQuery = true;
    options.SaveQueryOnSync = true;
    options.Endpoint = "/api/adhoc-reporting";
    options.StoreModelInCache = true;
    options.StoreQueryInCache = true;

    options.UseDbContextWithoutIdentity<AppDbContext>(loaderOptions => {
        //Ignore the "Reports" DbSet as well
        loaderOptions.AddFilter(entity => {
            return entity.ClrType != typeof(Report);
        });
    });

    // here we add our custom query store
    options.UseQueryStore((manager) => new ReportStore(manager.Services));

    options.UseDefaultAuthProvider((provider) => {
        //by default NewQuery, SaveQuery and RemoveQuery actions are accessible by the users with 'eq-manager' role 
        //here you can remove that requirement and make those actions available for all authorized users
        //provider.RequireAuthorization(EqAction.NewQuery, EqAction.SaveQuery, EqAction.RemoveQuery);

        //here is an example how you can make some actions accessible only by users with a particular role.
        //provider.RequireRole(DefaultEqAuthProvider.EqManagerRole, EqAction.NewQuery, EqAction.SaveQuery, EqAction.RemoveQuery);
    });

    options.AddPreFetchTunerWithHttpContext((manager, context) => {
        //the next two lines demonstrate how to add to each generated query a condition that filters data by the current user
        //string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //manager.Query.ExtraConditions.AddSimpleCondition("Employees.EmployeeID", "Equal", userId);
    });
});

app.MapRazorPages();

//Init demo database (if necessary)
app.EnsureDbInitializedAsync(builder.Configuration, app.Environment)
    .GetAwaiter()
    .GetResult();

app.Run();

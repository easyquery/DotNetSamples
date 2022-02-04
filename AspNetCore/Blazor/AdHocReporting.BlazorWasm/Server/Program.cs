using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using EasyData.Export;
using Korzh.EasyQuery.Services;

using EqDemo.Data;
using EqDemo.Models;
using EqDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("EqDemoSqLite"))
    //options.UseSqlServer(builder.Configuration.GetConnectionString("EqDemoDb"))
);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(opts => {
    opts.Password.RequiredLength = 4;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, AppDbContext>(options => {
        //the following 2 lines are necessary to support roles on the WebAssembly side
        options.IdentityResources["openid"].UserClaims.Add("role");
        options.ApiResources.Single().UserClaims.Add("role");
    });

// We need to do this as it maps "role" to ClaimTypes.Role and causes issues
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

//EasyQuery services
builder.Services.AddEasyQuery()
        .UseSqlManager()
        .AddDefaultExporters()
        .AddDataExporter<PdfDataExporter>("pdf")
        .AddDataExporter<ExcelDataExporter>("excel");

// add the service that generators default reports
// it's used for demonstration purposes only and can't be deleted in production
builder.Services.AddScoped<DefaultReportGenerator>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapEasyQuery(options => {
    options.DefaultModelId = "adhoc-reporting";
    options.SaveNewQuery = true;
    options.SaveQueryOnSync = true;
    options.Endpoint = "/api/adhoc-reporting";

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
app.MapControllers();
app.MapFallbackToFile("index.html");

//Init demo database (if necessary)
app.EnsureDbInitializedAsync(builder.Configuration, app.Environment).GetAwaiter().GetResult();

app.Run();

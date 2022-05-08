using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

using Korzh.EasyQuery.Services;

using EqDemo;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var DbConnectionString = configuration.GetConnectionString("EqDemoDb");

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlite(DbConnectionString);
    //options.UseSqlServer(configuration.GetConnectionString("EqDemoDb"));
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddEasyQuery();

builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
}
else {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapEasyQuery(options =>
{
    options.Endpoint = "/data-filtering";
    options.UseEntity((manager) => manager.Services
            .GetRequiredService<AppDbContext>()
            .Orders
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .AsQueryable());

    //options.DefaultModelId = "nwind";
    options.BuildQueryOnSync = true;
    options.SaveNewQuery = false;
});

app.MapRazorPages();

//Init demo database (if necessary)
app.EnsureDbInitialized(configuration, app.Environment);

app.Run();
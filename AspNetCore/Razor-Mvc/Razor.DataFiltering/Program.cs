using Microsoft.EntityFrameworkCore;

using EqDemo;
using Korzh.EasyQuery.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlServer(configuration.GetConnectionString("EqDemoDb"));
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
});

app.MapRazorPages();

//Init demo database (if necessary)
app.EnsureDbInitialized(configuration, app.Environment);

app.Run();
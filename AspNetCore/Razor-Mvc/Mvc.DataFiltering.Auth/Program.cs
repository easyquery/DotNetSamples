using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using EqDemo;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlite(configuration.GetConnectionString("EqDemoDb"));
    //options.UseSqlServer(configuration.GetConnectionString("EqDemoDb"));
});

// Setting up authentication/authorization services.
// Users and roles are stored in the database (via AppDbContext)
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

builder.Services.ConfigureApplicationCookie(options => {
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddEasyQuery();

builder.Services.AddControllersWithViews();

//Razor Pages are necessary for the default Identity UI (login/register pages)
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages();

//Init demo database (if necessary)
app.EnsureDbInitializedAsync(configuration, app.Environment)
    .GetAwaiter()
    .GetResult();

app.Run();

using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add logging to verify what’s going on
// builder.Logging.ClearProviders();
// builder.Logging.AddConsole();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<StoreDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration["ConnectionStrings:SportsStoreConnection"]);
});

builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();

builder.Services.AddRazorPages(); // sets up the services used by Razor Pages
builder.Services.AddDistributedMemoryCache(); //sets up the in-memory data store.
builder.Services.AddSession(); //registers the services used to access session data

//any Cart required by components handling the same HTTP request will receive the same object
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
//specifies that the same object should always be used.
// to use the HttpContextAccessor class when implementations of the IHttpContextAccessor interface are required.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//to enable the services and middleware for Blazor, creates the services that Blazor uses
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:IdentityConnection"]));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();

var app = builder.Build();

// app.MapGet("/", () => "Hello World!");
if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/error");
}
app.UseRequestLocalization(opts =>
{
    opts.AddSupportedCultures("en-US")
    .AddSupportedUICultures("en-US")
    .SetDefaultCulture("en-US");
});


app.UseStaticFiles();
app.UseSession(); // allows the session system to automatically associate requests with sessions when they arrive from the client

app.UseAuthentication();//to set up the middleware components that implement the security policy
app.UseAuthorization();//to set up the middleware components that implement the security policy

app.MapControllerRoute("catpage", "{category}/Page{productPage:int}",
    new { Controller = "Home", action = "Index" });

app.MapControllerRoute("page", "Page{productPage:int}",
    new { Controller = "Home", action = "Index", productPage = 1 });

app.MapControllerRoute("category", "{category}",
    new { Controller = "Home", action = "Index", productPage = 1 });

app.MapControllerRoute("pagination", "Products/Page{productPage}",
    new { Controller = "Home", action = "Index", productPage = 1 });

// app.UseRouting();
app.MapDefaultControllerRoute();
app.MapRazorPages(); // registers Razor Pages as endpoints that the URL routing system
app.MapBlazorHub(); //registers the Blazor middleware components
//to finesse the routing system to ensure that Blazor works seamlessly with the rest of the application
app.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");

SeedData.EnsurePopulated(app);
//dotnet ef database drop --force --context StoreDbContext   =>command in npowershell to restore the powershell
IdentitySeedData.EnsurePopulated(app);
//dotnet ef database drop --force --context AppIdentityDbContext   =>command in npowershell to restore the powershell

app.Run();

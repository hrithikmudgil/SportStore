using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

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

var app = builder.Build();

// app.MapGet("/", () => "Hello World!");

app.UseStaticFiles();

app.MapControllerRoute("pagination",
    "Products/Page{productPage}",
    new {Controller="Home",action="Index"});
// app.UseRouting();
app.MapDefaultControllerRoute();
SeedData.EnsurePopulated(app);
//dotnet ef database drop --force --context StoreDbContext   =>command in npowershell to restore the powershell

app.Run();

using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using StockApp;

var builder = WebApplication.CreateBuilder(args);

//Services
builder.Services.AddControllersWithViews();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));

//add services into IoC container
builder.Services.AddScoped<IStocksService, StocksService>();
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.AddHttpClient();

//Specification of the context
builder.Services.AddDbContext<StockMarketDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

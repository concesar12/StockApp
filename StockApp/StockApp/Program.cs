using StockApp.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
//By registering the FinnhubService class with a scoped lifetime, ASP.NET Core will create a new instance of the FinnhubService class for each scope that it is requested in.
//A scope is essentially a request or a unit of work in the application. This helps to ensure that each request or unit of work has its own instance of the FinnhubService class,
//which can help to prevent issues such as thread safety problems or data corruption.
builder.Services.AddScoped<FinnhubService>();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

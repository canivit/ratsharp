using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.Hubs;

var builder = WebApplication.CreateBuilder();
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole();
builder.Services.AddSignalR();
builder.Services.AddControllers();
var app = builder.Build();
app.MapHub<RatSharpHub>("/hub");
app.MapControllers();
app.Urls.Add("http://localhost:8080");

app.Run();
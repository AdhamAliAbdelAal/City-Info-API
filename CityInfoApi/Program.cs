using System.Runtime.InteropServices;
using CityInfoApi.DbContexts;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"Environment: {builder.Configuration["ASPNETCORE_ENVIRONMENT"]}");
builder.Host.UseSerilog();

// to remove the default logging providers
// builder.Logging.ClearProviders();

// add logging to the console
// builder.Logging.AddConsole();

// Add services to the container.

// add serilog to the container

/*
 The AddControllers method is an extension method that configures and registers the MVC framework services in the service collection. MVC is used for building web applications that follow the Model-View-Controller architectural pattern. When you call AddControllers, it registers services and components required for handling HTTP requests and routing them to the appropriate controller actions.
 */
builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
})
    .AddNewtonsoftJson() // this replace the default JSON serializer with Newtonsoft.Json
    .AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// the next two services are required for Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//to infer the content type of files
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();


builder.Services.AddDbContext<CityInfoDbContext>(options =>
    options.UseSqlite(builder.Configuration["ConnectionStrings:CityInfoDbContextConnection"] ?? string.Empty));
var app = builder.Build();
//
// // Configure the HTTP request pipeline.

Console.WriteLine(app.Environment.EnvironmentName);
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// UseHttpsRedirection is a middleware that redirects HTTP requests to HTTPS.
// It is required for the OpenAPI/Swagger middleware to work correctly.
app.UseHttpsRedirection();

app.UseRouting();
//useRouting is a middleware that sets up routing for the application.

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
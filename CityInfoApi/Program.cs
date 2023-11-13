using System.Text;
using CityInfoApi.DbContexts;
using CityInfoApi.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// AppDomain.CurrentDomain.GetAssemblies() this line is used to get all the assemblies in the curr

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration["Authentication:Audience"],
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretForKey"]??string.Empty))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustBeAdham", policy =>
    {
        policy.RequireAuthenticatedUser();
        // add policy to check if the sub claim is equal to adham
        policy.RequireClaim("name", "adham");
    });
});

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

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

/*
Concrete Types Injection: Use this approach when you have a fixed, well-defined implementation of a dependency that is unlikely to change, and you want a simple and direct way to create the object. It's often suitable for smaller applications or components where you don't need the flexibility of switching implementations.

Interface Injection: Use this approach when you want to decouple your code, improve testability, and make it easier to switch implementations of dependencies. It's especially useful in larger applications where you need to manage and configure the relationships between components dynamically. Interface injection is a fundamental concept in Inversion of Control (IoC) containers, which are widely used in complex and scalable software systems.

In practice, many software projects use a combination of both approaches, applying concrete types injection for certain components and interface injection for more flexible and dynamic parts of the system, depending on the specific requirements and design goals.
 */
 
 /*
the UseRouting() is the middleware that matches the url to an endpoint.

the UseEndpoints() is the middleware that actually executes the matched endpoint.

so any middleware after the UseRouting() and before UseEndpoints() can access the route data ( the controller name, action, etc) and will run before the endpoint.
  */
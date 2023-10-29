using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
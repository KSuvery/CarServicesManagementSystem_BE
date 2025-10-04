using CarServ.API.Configuration;
using CarServ.Repository.Repositories.DTO;
using CarServ.service.Services.Configuration;
using CarServ.service.WorkerService;
using CarServ.Service.Services;
using DotNetEnv;
using Microsoft.Extensions.AI;
using Serilog;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add configuration sources
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true) // optional: true allows fallback
    .AddUserSecrets<Program>(optional: true) // Only works if your project has user secrets enabled
    .AddEnvironmentVariables()
    .AddCommandLine(args);

Env.Load();
var config = builder.Configuration;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateLogger();


builder.Logging.AddSerilog();
builder.Services.AddSingleton(Log.Logger);
builder.Services.AddSingleton<Serilog.Extensions.Hosting.DiagnosticContext>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddDatabaseConfiguration(config);
builder.Services.AddServiceConfiguration(config);
builder.Services.AddRepositoryConfiguration(config);
builder.Services.AddJwtAuthenticationService(config);
builder.Services.AddThirdPartyServices(config);
builder.Services.AddSwaggerService();
builder.Services.Configure<AdminSettings>(builder.Configuration.GetSection("AdminCredentials"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<AdminSeederService>();
    await seeder.SeedAdminAsync();
    await seeder.SeedCustomerAsync();
}
/**/
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwaggerWithUtf8();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

using Serilog;
using CarServ.API.Configuration;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.OpenApi.Models;

using DotNetEnv;
using CarServ.Repository.Repositories.DTO;
using CarServ.service.WorkerService;
using CarServ.service.Services.Interfaces;
using CarServ.service.Services;
using System.Net.Mail;
using System.Net;
using CarServ.Service.Services;


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
});





// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen(option =>
//{
//    option.DescribeAllParametersInCamelCase();
//    option.ResolveConflictingActions(conf => conf.First());
//    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Description = "Please enter a valid token",
//        Name = "Authorization",
//        Type = SecuritySchemeType.Http,
//        BearerFormat = "JWT",
//        Scheme = "Bearer"
//    });
//    option.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type=ReferenceType.SecurityScheme,
//                    Id="Bearer"
//                }
//            },
//            new string[]{}
//        }
//    });
//});


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<AdminSeederService>();
    await seeder.SeedAdminAsync();
    await seeder.SeedCustomerAsync();
}

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

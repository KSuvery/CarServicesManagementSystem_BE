using Serilog;
using CarServ.API.Configuration;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.OpenApi.Models;

using DotNetEnv;
using CarServ.Repository.Repositories.DTO;
using CarServ.Service.WorkerService;
using CarServ.Service.Services.Interfaces;
using CarServ.Service.Services;


var builder = WebApplication.CreateBuilder(args);
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
/*builder.Services.AddThirdPartyServices(config);*/
builder.Services.AddSwaggerService();
builder.Services.Configure<AdminSettings>(builder.Configuration.GetSection("AdminCredentials"));


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
});





// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

//Auto-generate new admin with admin credentials is in appsetting
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<AdminSeederService>();
    await seeder.SeedCustomerAsync();
    await seeder.SeedAdminAsync();
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

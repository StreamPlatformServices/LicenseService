using APIGatewayMain.ServiceCollectionExtensions;
using APIGatewayMain.ServiceCollectionExtensions.ComponentsExtensions;
using LicenseService.Configuration;
using LicenseService.Persistance;
using LicenseService.Persistance.Repositories;
using LicenseService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configPath = Path.GetFullPath(builder.Configuration["ConfigPath"] ?? Directory.GetCurrentDirectory());
var databasePath = Path.GetFullPath(builder.Configuration["DatabasePath"] ?? Directory.GetCurrentDirectory());

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(Path.Combine(configPath, "appsettings.json"), optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddCommonConfiguration(builder.Configuration);

var kestrelSettings = builder.Configuration.GetSection("KestrelSettings").Get<KestrelSettings>() ?? throw new Exception("Fatal error: Please provide kestrel configuration");
builder.AddKestrelSettings(kestrelSettings);

// Add services to the container.
builder.Services.AddDbContext<LicenseDatabaseContext>(options =>
            options.UseSqlite($"Data Source={databasePath}/license.db"));

//TODO: AddKeyServiceCLient?? (change name of API components to Client???)
builder.Services.AddKeyServiceAPI();
builder.Services.AddScoped<ILicenseRepository, LicenseRepository>();
builder.Services.AddScoped<ILicenseFasade, LicenseFasade>();

var corsPolicyName = "CustomCorsPolicy";
var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>() ?? throw new Exception("Fatal error: Please provide CorsSettings configuration");
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName,
        policy =>
        {
            policy.WithOrigins(corsSettings.AllowedHosts)
                .WithHeaders(corsSettings.AllowedHeaders)
                .WithMethods(corsSettings.AllowedMethods);
        });
});

builder.Services.AddControllers();
var useSwagger = builder.Configuration.GetSection("UseSwagger").Get<bool>();
if (useSwagger)
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (useSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LicenseService API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors(corsPolicyName);

if (kestrelSettings.UseTls)
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

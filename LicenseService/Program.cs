using APIGatewayMain.ServiceCollectionExtensions;
using APIGatewayMain.ServiceCollectionExtensions.ComponentsExtensions;
using LicenseService.Persistance;
using LicenseService.Persistance.Repositories;
using LicenseService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCommonConfiguration(builder.Configuration);

// Add services to the container.
builder.Services.AddDbContext<LicenseDatabaseContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//TODO: AddKeyServiceCLient?? (change name of API components to Client???)
builder.Services.AddKeyServiceAPI();
builder.Services.AddScoped<ILicenseRepository, LicenseRepository>();
builder.Services.AddScoped<ILicenseFasade, LicenseFasade>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

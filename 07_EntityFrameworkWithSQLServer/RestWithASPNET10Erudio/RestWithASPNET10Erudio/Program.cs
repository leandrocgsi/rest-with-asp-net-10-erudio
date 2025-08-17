using RestWithASPNET10Erudio.Configurations;
using RestWithASPNET10Erudio.Services;
using RestWithASPNET10Erudio.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddScoped<IPersonServices, PersonServicesImpl>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

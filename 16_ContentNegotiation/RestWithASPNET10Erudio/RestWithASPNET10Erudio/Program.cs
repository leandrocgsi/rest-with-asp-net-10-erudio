using RestWithASPNET10Erudio.Configurations;
using RestWithASPNET10Erudio.Repositories;
using RestWithASPNET10Erudio.Repositories.Impl;
using RestWithASPNET10Erudio.Services;
using RestWithASPNET10Erudio.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();

builder.Services.AddControllers()
    .AddContentNegotiation();

builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddEvolveConfiguration(builder.Configuration, builder.Environment);

builder.Services.AddScoped<IPersonServices, PersonServicesImpl>();
builder.Services.AddScoped<IBookServices, BookServicesImpl>();
builder.Services.AddScoped<PersonServicesImplV2>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

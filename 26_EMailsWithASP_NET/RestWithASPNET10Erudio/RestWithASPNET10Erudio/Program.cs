using RestWithASPNET10Erudio.Configurations;
using RestWithASPNET10Erudio.Files.Exporters.Factory;
using RestWithASPNET10Erudio.Files.Exporters.Impl;
using RestWithASPNET10Erudio.Files.Importers.Factory;
using RestWithASPNET10Erudio.Files.Importers.Impl;
using RestWithASPNET10Erudio.Hypermedia.Filters;
using RestWithASPNET10Erudio.Mail;
using RestWithASPNET10Erudio.Repositories;
using RestWithASPNET10Erudio.Repositories.Impl;
using RestWithASPNET10Erudio.Services;
using RestWithASPNET10Erudio.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<HypermediaFilter>();
    })
    .AddContentNegotiation();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenAPIConfig();
builder.Services.AddSwaggerConfig();
builder.Services.AddRouteConfig();

builder.Services.AddCorsConfiguration(builder.Configuration);
builder.Services.AddHATEOASConfiguration();

builder.Services.AddEmailConfiguration(builder.Configuration);

builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddEvolveConfiguration(builder.Configuration, builder.Environment);

builder.Services.AddScoped<IPersonServices, PersonServicesImpl>();
builder.Services.AddScoped<IBookServices, BookServicesImpl>();
builder.Services.AddScoped<PersonServicesImplV2>();

builder.Services.AddScoped<IEmailService, EmailServiceImpl>();
builder.Services.AddScoped<EmailSender>();

builder.Services.AddScoped<CsvImporter>();
builder.Services.AddScoped<XlsxImporter>();
builder.Services.AddScoped<FileImporterFactory>();

builder.Services.AddScoped<CsvExporter>();
builder.Services.AddScoped<XlsxExporter>();
builder.Services.AddScoped<FileExporterFactory>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
builder.Services.AddScoped<IFileServices, FileServicesImpl>();

builder.Services.AddScoped<IPersonRepository, PersonRepository>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRouting();
//app.UseCorsConfiguration();
app.UseCorsConfiguration(builder.Configuration);

app.MapControllers();

app.UseHATEOASRoutes();

app.UseSwaggerSpecification();
app.UseScalarConfiguration();

app.Run();

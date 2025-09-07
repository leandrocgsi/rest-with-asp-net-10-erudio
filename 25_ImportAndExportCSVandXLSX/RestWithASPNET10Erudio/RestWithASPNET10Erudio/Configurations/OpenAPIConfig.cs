using Microsoft.OpenApi.Models;

namespace RestWithASPNET10Erudio.Configurations
{
    public static class OpenAPIConfig
    {
        private static readonly string AppName = "ASP.NET 2026 REST API's from 0 to Azure and GCP with .NET 10, Docker e Kubernetes";
        private static readonly string AppDescription = $"REST API RESTful developed in course {AppName}";

        public static IServiceCollection AddOpenAPIConfig(
            this IServiceCollection services)
        {
            services.AddSingleton(new OpenApiInfo
            {
                Title = AppName,
                Version = "v1",
                Description = AppDescription,
                Contact = new OpenApiContact
                {
                    Name = "Erudio",
                    Url = new Uri("https://pub.erudio.com.br/meus-cursos")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT",
                    Url = new Uri("https://pub.erudio.com.br/meus-cursos")
                }
            });
            return services;
        }
    }
}

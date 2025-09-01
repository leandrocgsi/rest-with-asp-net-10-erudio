using Scalar.AspNetCore;

namespace RestWithASPNET10Erudio.Configurations
{
    public static class ScalarConfig
    {
        private static readonly string AppName = "ASP.NET 2026 REST API's from 0 to Azure and GCP with .NET 10, Docker e Kubernetes";

        public static WebApplication UseScalarConfiguration(
            this WebApplication app)
        {
            app.MapScalarApiReference("/scalar", options =>
            {
                options
                    .WithTitle(AppName)
                    .WithOpenApiRoutePattern("/swagger/v1/swagger.json");
            });
            return app;
        }
    }
}

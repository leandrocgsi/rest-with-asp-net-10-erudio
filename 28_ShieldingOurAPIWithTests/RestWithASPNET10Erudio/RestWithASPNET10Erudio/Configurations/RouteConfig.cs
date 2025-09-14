namespace RestWithASPNET10Erudio.Configurations
{
    public static class RouteConfig
    {
        public static IServiceCollection AddRouteConfig(
            this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options => {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
            return services;
        }
    }
}

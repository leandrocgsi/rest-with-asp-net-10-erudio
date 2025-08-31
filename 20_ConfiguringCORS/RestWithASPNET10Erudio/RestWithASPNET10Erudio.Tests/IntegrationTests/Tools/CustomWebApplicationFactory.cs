using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests.Tools
{
    public class CustomWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {
        private readonly string _connectionString;

        public CustomWebApplicationFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void ConfigureWebHost(
            IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var dict = new Dictionary<string, string>
                {
                    {
                        "MSSQLServerSQLConnection:MSSQLServerSQLConnectionString",
                        _connectionString
                    }
                };
                config.AddInMemoryCollection(dict!);
            });
        }
    }
}

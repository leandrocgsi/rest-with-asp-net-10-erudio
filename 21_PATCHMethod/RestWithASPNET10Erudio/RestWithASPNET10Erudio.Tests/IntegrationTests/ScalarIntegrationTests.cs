using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RestWithASPNET10Erudio.Tests.IntegrationTests.Tools;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests
{
    public class ScalarIntegrationTests
        : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _httpClient;

        public ScalarIntegrationTests(SqlServerFixture sqlFixture)
        {
            var factory = new CustomWebApplicationFactory<Program>(
                sqlFixture.ConnectionString);

            _httpClient = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    BaseAddress = new Uri("http://localhost")
                }
            );
        }

        [Fact]
        public async Task Scalar_ShouldReturnScalarUI()
        {
            // Arrange & Act
            var response = await _httpClient
                .GetAsync("/scalar/");

            // Assert
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("<title>ASP.NET 2026 REST API's from 0 to Azure and GCP with .NET 10, Docker e Kubernetes</title>");
            content.Should().Contain("script src");
        }
    }
}

using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Tests.IntegrationTests.Tools;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests.CORS
{
    [TestCaseOrderer(
        TestConfigs.TestCaseOrdererFullName,
        TestConfigs.TestCaseOrdererAssembly)]
    public class PersonCorsIntegrationTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _httpClient;
        private static PersonDTO? _person;
        private static TokenDTO? _token;

        public PersonCorsIntegrationTests(SqlServerFixture sqlFixture)
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

        private void AddOriginHeader(string origin)
        {
            _httpClient.DefaultRequestHeaders.Remove("Origin");
            _httpClient.DefaultRequestHeaders.Add("Origin", origin);
        }

        [Fact(DisplayName = "00 - Sign In")]
        [TestPriority(0)]
        public async Task SignIn_ShouldReturnToken()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);

            var credentials = new UserDTO
            {
                Username = "leandro",
                Password = "admin123"
            };

            // Act
            var response = await _httpClient
                .PostAsJsonAsync("api/auth/signin", credentials);

            // Assert
            response.EnsureSuccessStatusCode();

            var token = await response.Content
                .ReadFromJsonAsync<TokenDTO>();

            token.Should().NotBeNull();

            token.AccessToken.Should().NotBeNullOrWhiteSpace();
            token.RefreshToken.Should().NotBeNullOrWhiteSpace();

            _token = token;
        }

        [Fact(DisplayName = "01 - Create Person With Allowed Origin")]
        [TestPriority(1)]
        public async Task CreatePerson_WithAllowedOrigin_ShouldReturnCreated()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);

            AddOriginHeader("https://erudio.com.br");

            var request = new PersonDTO
            {
                FirstName = "Richard",
                LastName = "Stallman",
                Address = "New York City - New York - USA",
                Gender = "Male"
            };

            // Act
            var response = await _httpClient
                .PostAsJsonAsync("api/person/v1", request);

            // Assert
            response.EnsureSuccessStatusCode();

            var created = await response.Content
                .ReadFromJsonAsync<PersonDTO>();
            created.Should().NotBeNull();
            created.Id.Should().BeGreaterThan(0);

            _person = created;
        }

        [Fact(DisplayName = "02 - Create Person With Disallowed Origin")]
        [TestPriority(2)]
        public async Task CreatePerson_WithDisallowedOrigin_ShouldReturnForbiden()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);

            AddOriginHeader("https://semeru.com.br");

            var request = new PersonDTO
            {
                FirstName = "Richard",
                LastName = "Stallman",
                Address = "New York City - New York - USA",
                Gender = "Male"
            };

            // Act
            var response = await _httpClient
                .PostAsJsonAsync("api/person/v1", request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

            var content = await response.Content
                .ReadAsStringAsync();
            content.Should().Be("CORS origin not allowed.");
        }

        [Fact(DisplayName = "03 - Get Person By ID With Allowed Origin")]
        [TestPriority(3)]
        public async Task FindPersonById_WithAllowedOrigin_ShouldReturnOk()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);

            AddOriginHeader("https://erudio.com.br");

            // Act
            var response = await _httpClient
                .GetAsync($"api/person/v1/{_person.Id}");

            // Assert
            response.EnsureSuccessStatusCode();

            var found = await response.Content
                .ReadFromJsonAsync<PersonDTO>();

            found.Should().NotBeNull();
            found.Id.Should().Be(_person.Id);
            found.FirstName.Should().Be("Richard");
            found.LastName.Should().Be("Stallman");
            found.Address.Should().Be("New York City - New York - USA");
        }

        [Fact(DisplayName = "04 - Get Person By ID With Disallowed Origin")]
        [TestPriority(4)]
        public async Task FindByIdPerson_WithDisallowedOrigin_ShouldReturnForbiden()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);

            AddOriginHeader("https://semeru.com.br");

            // Act
            var response = await _httpClient
                .GetAsync($"api/person/v1/{_person.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            var content = await response.Content
                .ReadAsStringAsync();
            content.Should().Be("CORS origin not allowed.");
        }
    }
}

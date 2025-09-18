using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Tests.IntegrationTests.Tools;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests.Auth
{
    [TestCaseOrderer(
        TestConfigs.TestCaseOrdererFullName,
        TestConfigs.TestCaseOrdererAssembly)]
    public class AuthControllerIntegrationTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _httpClient;
        private static TokenDTO? _token;
        private static AccountCredentialsDTO? _createdUser;

        public AuthControllerIntegrationTests(SqlServerFixture sqlFixture)
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

        [Fact(DisplayName = "01 - Create User")]
        [TestPriority(1)]
        public async Task CreateUser_ShouldReturnCreatedUser()
        {
            // Arrange
            var request = new AccountCredentialsDTO
            {
                Username = "solomon",
                Fullname = "Solomon Hykes",
                Password = "hykes123"
            };

            // Act
            var response = await _httpClient
                .PostAsJsonAsync("api/auth/create", request);

            // Assert
            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<AccountCredentialsDTO>();

            result.Should().NotBeNull();
            result.Username.Should().Be("solomon");
            result.Fullname.Should().Be("Solomon Hykes");

            _createdUser = result;
        }

        [Fact(DisplayName = "02 - Sign In")]
        [TestPriority(2)]
        public async Task SignIn_ShouldReturnToken()
        {
            // Arrange
            var credentials = new UserDTO
            {
                Username = "solomon",
                Password = "hykes123"
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

        [Fact(DisplayName = "03 - Refresh Token")]
        [TestPriority(3)]
        public async Task RefreshToken_ShouldReturnNewToken()
        {
            // Act
            var response = await _httpClient
                .PostAsJsonAsync("api/auth/refresh", _token);

            // Assert
            response.EnsureSuccessStatusCode();

            var newToken = await response.Content
                .ReadFromJsonAsync<TokenDTO>();

            newToken.Should().NotBeNull();
            newToken.AccessToken.Should().NotBeNullOrWhiteSpace();
            newToken.RefreshToken.Should().NotBeNullOrWhiteSpace();

            newToken.AccessToken.Should().NotBeSameAs(_token?.AccessToken);
            newToken.RefreshToken.Should().NotBeSameAs(_token?.RefreshToken);

            _token = newToken;
        }

        [Fact(DisplayName = "04 - Revoke Token")]
        [TestPriority(4)]
        public async Task RevokeToken_ShouldReturnNoContent()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);

            // Act
            var response = await _httpClient
                .PostAsync("api/auth/revoke", null);

            // Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "05 - Sign In with Invalid Credentials ShouldReturnUnauthorized")]
        [TestPriority(5)]
        public async Task SignIn_WithInvalidCredentials_ShouldReturnUnauthorized()
        {
            // Arrange
            var credentials = new UserDTO
            {
                Username = "solomon",
                Password = "wrongpassword"
            };

            // Act
            var response = await _httpClient
                .PostAsJsonAsync("api/auth/signin", credentials);

            // Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.Unauthorized);
        }

        [Fact(DisplayName = "06 - Revoke Token without Authorization Header ShouldReturnUnauthorized")]
        [TestPriority(6)]
        public async Task RevokeToken_WithoutAuthorizationHeader_ShouldReturnUnauthorized()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            // Act
            var response = await _httpClient
                .PostAsync("api/auth/revoke", null);

            // Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.Unauthorized);
        }
    }
}

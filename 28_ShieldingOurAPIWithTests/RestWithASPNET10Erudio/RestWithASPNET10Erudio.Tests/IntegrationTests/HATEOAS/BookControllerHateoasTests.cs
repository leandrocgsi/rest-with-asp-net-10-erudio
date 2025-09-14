using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Tests.IntegrationTests.Tools;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests.HATEOAS
{
    [TestCaseOrderer(
        TestConfigs.TestCaseOrdererFullName,
        TestConfigs.TestCaseOrdererAssembly)]
    public class BookControllerHateoasTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _httpClient;
        private static BookDTO? _book;
        private static TokenDTO? _token;

        public BookControllerHateoasTests(SqlServerFixture sqlFixture)
        {
            var factory = new CustomWebApplicationFactory<Program>(sqlFixture.ConnectionString);
            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost")
            });
        }

        private void AssertLinkPattern(string content, string rel)
        {
            var pattern = $@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/book/v1.*?""";
            Regex.IsMatch(content, pattern).Should().BeTrue($"Link with rel='{rel}' should exist and have valid href");
        }


        [Fact(DisplayName = "00 - Sign In")]
        [TestPriority(0)]
        public async Task SignIn_ShouldReturnToken()
        {
            // Arrange
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

        [Fact(DisplayName = "01 - Create Book")]
        [TestPriority(1)]
        public async Task CreateBook_ShouldContainHateoasLinks()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);

            var request = new BookDTO
            {
                Title = "Docker Deep Dive",
                Author = "Nigel Poulton",
                Price = 54.99M,
                LaunchDate = DateTime.Now
            };

            var response = await _httpClient.PostAsJsonAsync("api/book/v1", request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _book = await response.Content.ReadFromJsonAsync<BookDTO>();

            AssertLinkPattern(content, "collection");
            AssertLinkPattern(content, "self");
            AssertLinkPattern(content, "create");
            AssertLinkPattern(content, "update");
            AssertLinkPattern(content, "delete");
        }

        [Fact(DisplayName = "02 - Update Book")]
        [TestPriority(2)]
        public async Task UpdateBook_ShouldContainHateoasLinks()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);

            _book!.Title = "Docker Deep Dive - 2° Edition";

            // Act
            var response = await _httpClient.PutAsJsonAsync("api/book/v1", _book);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _book = await response.Content.ReadFromJsonAsync<BookDTO>();

            AssertLinkPattern(content, "collection");
            AssertLinkPattern(content, "self");
            AssertLinkPattern(content, "create");
            AssertLinkPattern(content, "update");
            AssertLinkPattern(content, "delete");
        }

        [Fact(DisplayName = "03 - Get Book By ID")]
        [TestPriority(3)]
        public async Task GetBookById_ShouldContainHateoasLinks()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);

            // Act
            var response = await _httpClient.GetAsync($"api/book/v1/{_book?.Id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            AssertLinkPattern(content, "collection");
            AssertLinkPattern(content, "self");
            AssertLinkPattern(content, "create");
            AssertLinkPattern(content, "update");
            AssertLinkPattern(content, "delete");
        }

        [Fact(DisplayName = "04 - Delete Book By ID")]
        [TestPriority(4)]
        public async Task DeleteBookById_ShouldReturnNoContent()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);
            
            // Act
            var response = await _httpClient.DeleteAsync($"api/book/v1/{_book?.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "05 - Find All Books")]
        [TestPriority(5)]
        public async Task FindAll_ShouldReturnLinksForEachBook()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);

            // Act
            var response = await _httpClient.GetAsync("api/book/v1");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            // Regex para capturar todos os "id" do retorno
            var idMatches = Regex.Matches(content, @"""id"":\s*(\d+)");
            idMatches.Count.Should().BeGreaterThan(0, "There should be at least one book");

            foreach (Match match in idMatches)
            {
                var id = match.Groups[1].Value;

                // Validar cada link do item
                var expectedRels = new[] { "collection", "self", "create", "update", "delete" };

                foreach (var rel in expectedRels)
                {
                    var pattern = rel switch
                    {
                        "self" or "delete" => $@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/book/v1/{id}""",
                        _ => $@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/book/v1"""
                    };

                    Regex.IsMatch(content, pattern, RegexOptions.IgnoreCase)
                         .Should().BeTrue($"Link '{rel}' should exist for book {id}");

                    // Validar que existe type
                    var typePattern = $@"""rel"":\s*""{rel}"".*?""type"":\s*""[^""]+""";
                    Regex.IsMatch(content, typePattern).Should().BeTrue($"Link '{rel}' must have a type for book {id}");
                }
            }
        }
    }
}
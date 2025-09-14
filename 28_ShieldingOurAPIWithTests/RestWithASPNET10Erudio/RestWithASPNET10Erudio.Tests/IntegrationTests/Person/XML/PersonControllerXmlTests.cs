using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Hypermedia.Utils;
using RestWithASPNET10Erudio.Tests.IntegrationTests.Tools;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests.Person.XML
{
    [TestCaseOrderer(
        TestConfigs.TestCaseOrdererFullName,
        TestConfigs.TestCaseOrdererAssembly)]
    public class PersonControllerXmlTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _httpClient;
        private static PersonDTO? _person;
        private static TokenDTO? _token;

        public PersonControllerXmlTests(SqlServerFixture sqlFixture)
        {
            var factory = new CustomWebApplicationFactory<Program>(
                sqlFixture.ConnectionString);

            _httpClient = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    BaseAddress = new Uri("http://localhost")
                }
            );

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));
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

            var content = XmlHelper.SerializeToXml(credentials);

            // Act
            var response = await _httpClient
                .PostAsync("api/auth/signin", content);

            // Assert
            response.EnsureSuccessStatusCode();

            var token = await XmlHelper
                .ReadFromXmlAsync<TokenDTO>(response);

            token.Should().NotBeNull();

            token.AccessToken.Should().NotBeNullOrWhiteSpace();
            token.RefreshToken.Should().NotBeNullOrWhiteSpace();

            _token = token;
        }

        [Fact(DisplayName = "01 - Create Person")]
        [TestPriority(1)]
        public async Task CreatePerson_ShouldReturnCreatedPerson()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);

            var request = new PersonDTO
            {
                FirstName = "Linus",
                LastName = "Torvalds",
                Address = "Helsinki - Finland",
                Gender = "Male",
                Enabled = true
            };

            // Act
            var response = await _httpClient
                .PostAsync("api/person/v1",
                XmlHelper.SerializeToXml(request));

            // Assert
            response.EnsureSuccessStatusCode();

            var created = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

            created.Should().NotBeNull();
            created.Id.Should().BeGreaterThan(0);
            created.FirstName.Should().Be("Linus");
            created.LastName.Should().Be("Torvalds");
            created.Address.Should().Be("Helsinki - Finland");
            created.Enabled.Should().BeTrue();

            _person = created;
        }

        [Fact(DisplayName = "02 - Update Person")]
        [TestPriority(2)]
        public async Task UpdatePerson_ShouldReturnUpdatedPerson()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);

            _person?.LastName = "Benedict Torvalds";

            // Act
            var response = await _httpClient
                .PutAsync("api/person/v1",
                XmlHelper.SerializeToXml(_person));

            // Assert
            response.EnsureSuccessStatusCode();

            var updated = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

            updated.Should().NotBeNull();
            updated.Id.Should().BeGreaterThan(0);
            updated.FirstName.Should().Be("Linus");
            updated.LastName.Should().Be("Benedict Torvalds");
            updated.Address.Should().Be("Helsinki - Finland");
            updated.Enabled.Should().BeTrue();

            _person = updated;
        }

        [Fact(DisplayName = "03 - Disable Person By ID")]
        [TestPriority(3)]
        public async Task DisablePersonById_ShouldReturnDisabledPerson()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);
            
            // Act
            var response = await _httpClient
                .PatchAsync($"api/person/v1/{_person?.Id}", null);

            // Assert
            response.EnsureSuccessStatusCode();

            var disabled = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

            disabled.Should().NotBeNull();
            disabled.Id.Should().BeGreaterThan(0);
            disabled.FirstName.Should().Be("Linus");
            disabled.LastName.Should().Be("Benedict Torvalds");
            disabled.Address.Should().Be("Helsinki - Finland");
            disabled.Enabled.Should().BeFalse(); 

            _person = disabled;
        }

        [Fact(DisplayName = "04 - Get Person By ID")]
        [TestPriority(4)]
        public async Task GetPersonById_ShouldReturnPerson()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);

            // Act
            var response = await _httpClient
                .GetAsync($"api/person/v1/{_person?.Id}");

            // Assert
            response.EnsureSuccessStatusCode();

            var found = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

            found.Should().NotBeNull();
            found.Id.Should().Be(_person?.Id);
            found.FirstName.Should().Be("Linus");
            found.LastName.Should().Be("Benedict Torvalds");
            found.Address.Should().Be("Helsinki - Finland");
            found.Enabled.Should().BeFalse();
        }

        [Fact(DisplayName = "05 - Delete Person By ID")]
        [TestPriority(5)]
        public async Task DeletePersonById_ShouldReturnNoContent()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);

            // Act
            var response = await _httpClient
                .DeleteAsync($"api/person/v1/{_person?.Id}");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "06 - Find all Person")]
        [TestPriority(6)]
        public async Task FindAllPerson_ShouldReturnListOfPerson()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue
                    ("Bearer", _token?.AccessToken);

            // Act
            var response = await _httpClient
                .GetAsync("api/person/v1/asc/10/1");
            // <-- sortDirection=asc, pageSize=10, page=1

            // Assert
            response.EnsureSuccessStatusCode();

            var page = await XmlHelper
                .ReadFromXmlAsync<PagedSearchDTO<PersonDTO>>(response);

            page.Should().NotBeNull();
            page.CurrentPage.Should().Be(1);

            var list = page?.List;

            list.Should().NotBeNull();
            list.Count.Should().BeGreaterThan(0);

            var first = list.First(p => p.FirstName == "Abbie");
            first.LastName.Should().Be("Bassford");
            first.Address.Should().Be("PO Box 88145");
            first.Enabled.Should().BeFalse();
            first.Gender.Should().Be("Male");

            var third = list.First(p => p.FirstName == "Abner");
            third.LastName.Should().Be("Castilla");
            third.Address.Should().Be("8th Floor");
            third.Enabled.Should().BeFalse();
            third.Gender.Should().Be("Male");

            page!.CurrentPage.Should().BeGreaterThan(0);
            page.TotalResults.Should().BeGreaterThan(0);
            page.PageSize.Should().BeGreaterThan(0);
            page.SortDirections.Should().NotBeNull();
        }
    }
}

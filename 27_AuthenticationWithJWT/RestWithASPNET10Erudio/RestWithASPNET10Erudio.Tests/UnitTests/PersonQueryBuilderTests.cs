
using FluentAssertions;
using Org.BouncyCastle.Asn1.X509;
using RestWithASPNET10Erudio.Repositories.QueryBuilders;

namespace RestWithASPNET10Erudio.Tests.UnitTests
{
    public class PersonQueryBuilderTests
    {
        private readonly PersonQueryBuilder _queryBuilder;

        public PersonQueryBuilderTests()
        {
            _queryBuilder = new PersonQueryBuilder();
        }

        [Fact]
        public void BuildQueries_ShouldReturnCorrectQueries()
        {
            // Arrange
            var name = "John";
            var sortDirection = "asc";
            var pageSize = 10;
            var page = 2;

            // Act
            var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(
                name, sortDirection, pageSize, page);

            // Assert
            query.Should().Contain("SELECT *  FROM person p WHERE 1 = 1  AND (p.first_name LIKE '%John%')");
            query.Should().Contain("ORDER BY p.first_name asc");
            query.Should().Contain("OFFSET 10 ROWS FETCH NEXT 10 ROWS ONLY");

            countQuery.Should().Be("SELECT COUNT(*)  FROM person p WHERE 1 = 1  AND (p.first_name LIKE '%John%') ");
            sort.Should().Be("asc");
            size.Should().Be(10);
            offset.Should().Be(10);
        }

        [Fact]
        public void BuildQueries_ShouldHandleInvalidPageAndPageSize()
        {
            // Arrange
            var name = "";
            var sortDirection = "desc";
            var pageSize = 0; // Invalid page size
            var page = -1;    // Invalid page number

            // Act
            var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(
                name, sortDirection, pageSize, page);

            // Assert
            query.Should().Contain("SELECT *  FROM person p WHERE 1 = 1 ");
            query.Should().Contain("ORDER BY p.first_name desc");
            query.Should().Contain("OFFSET 0 ROWS FETCH NEXT 1 ROWS ONLY");

            countQuery.Should().Be("SELECT COUNT(*)  FROM person p WHERE 1 = 1 ");
            sort.Should().Be("desc");
            size.Should().Be(1); // Page size should be set to minimum of 1
            offset.Should().Be(0); // Offset should be 0 for page 1
        }

        [Fact]
        public void BuildQueries_ShouldHandleNullOrWhitespaceName()
        {
            // Arrange
            string name = null; // Null name
            var sortDirection = "asc";
            var pageSize = 5;
            var page = 1;
            // Act
            var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(
                name, sortDirection, pageSize, page);
            // Assert
            query.Should().Contain("SELECT *  FROM person p WHERE 1 = 1 ");
            query.Should().Contain("ORDER BY p.first_name asc");
            query.Should().Contain("OFFSET 0 ROWS FETCH NEXT 5 ROWS ONLY");
            query.Should().NotContain("AND (p.first_name LIKE");

            countQuery.Should().Be("SELECT COUNT(*)  FROM person p WHERE 1 = 1 ");
            sort.Should().Be("asc");
            size.Should().Be(5);
            offset.Should().Be(0);
        }

        [Fact]
        public void BuildQueries_ShouldDefaultToDescForInvalidSortDirection()
        {
            // Arrange
            var name = "Jane";
            var sortDirection = "invalid"; // Invalid sort direction
            var pageSize = 10;
            var page = 1;

            // Act
            var (query, countQuery, sort, size, offset) = _queryBuilder.BuildQueries(
                name, sortDirection, pageSize, page);

            // Assert
            query.Should().Contain("SELECT *  FROM person p WHERE 1 = 1  AND (p.first_name LIKE '%Jane%')");
            query.Should().Contain("ORDER BY p.first_name asc"); // Should default to asc
            query.Should().Contain("OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY");

            countQuery.Should().Be("SELECT COUNT(*)  FROM person p WHERE 1 = 1  AND (p.first_name LIKE '%Jane%') ");
            sort.Should().Be("asc"); // Should be asc
            size.Should().Be(10);
            offset.Should().Be(0);
        }
    }
}

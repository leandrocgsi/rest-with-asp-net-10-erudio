using FluentAssertions;
using RestWithASPNET10Erudio.Data.Converter.Impl;
using RestWithASPNET10Erudio.Data.DTO.V2;
using RestWithASPNET10Erudio.Model;

namespace RestWithASPNET10Erudio.Tests.UnitTests
{
    public class PersonConverterTests
    {
        private readonly PersonConverter _converter;

        public PersonConverterTests()
        {
            _converter = new PersonConverter();
        }

        // PersonDTO to Person conversion tests
        [Fact]
        public void Parse_ShouldConvertPersonDTOToPerson()
        {
            // Arrange: prepare the data, objects, and dependencies required for the test
            var dto = new PersonDTO
            {
                Id = 1,
                FirstName = "Mahatma",
                LastName = "Gandhi",
                Address = "Porbandar - India",
                Gender = "Male",
                BirthDay = new DateTime(1869, 10, 2)
            };

            var expectedPerson = new Person
            {
                Id = 1,
                FirstName = "Mahatma",
                LastName = "Gandhi",
                Address = "Porbandar - India",
                Gender = "Male"
            };

            // Act: execute the method or functionality under test
            var person = _converter.Parse(dto);

            // Assert: verify that the result matches the expected outcome
            person.Should().NotBeNull();
            person.Id.Should().Be(expectedPerson.Id);
            person.FirstName.Should().Be(expectedPerson.FirstName);
            person.LastName.Should().Be(expectedPerson.LastName);
            person.Gender.Should().Be(expectedPerson.Gender);
            person.Should()
                .BeEquivalentTo(expectedPerson);
        }

        [Fact]
        public void Parse_NullPersonDTOShouldReturnNull()
        {
            PersonDTO dto = null;
            var person = _converter.Parse(dto);
            person.Should().BeNull();
        }

        // Person ← PersonDTO conversion tests
        [Fact]
        public void Parse_ShouldConvertPersonToPersonDTO()
        {
            // Arrange: prepare the data, objects, and dependencies required for the test
            var entity = new Person
            {
                Id = 1,
                FirstName = "Mahatma",
                LastName = "Gandhi",
                Address = "Porbandar - India",
                Gender = "Male",
                //BirthDay = new DateTime(1869, 10, 2)
            };

            var expectedPerson = new PersonDTO
            {
                Id = 1,
                FirstName = "Mahatma",
                LastName = "Gandhi",
                Address = "Porbandar - India",
                Gender = "Male"
            };

            // Act: execute the method or functionality under test
            var person = _converter.Parse(entity);

            // Assert: verify that the result matches the expected outcome
            person.Should().NotBeNull();
            person.Id.Should().Be(expectedPerson.Id);
            person.FirstName.Should().Be(expectedPerson.FirstName);
            person.LastName.Should().Be(expectedPerson.LastName);
            person.Gender.Should().Be(expectedPerson.Gender);
            person.Should()
                .BeEquivalentTo(expectedPerson,
                options => options.Excluding(person => person.BirthDay));
            person.BirthDay.Should().NotBeNull();
        }

        [Fact]
        public void Parse_NullPersonShouldReturnNull()
        {
            Person dto = null;
            var person = _converter.Parse(dto);
            person.Should().BeNull();
        }

        [Fact]
        public void ParseList_ShouldConvertPersonDTOListToPersonList()
        {
            // Arrange
            var dtoList = new List<PersonDTO>
            {
                new PersonDTO
                {
                    Id = 1,
                    FirstName = "Mahatma",
                    LastName = "Gandhi",
                    Address = "Porbandar - India",
                    Gender = "Male",
                    BirthDay = new DateTime(1869, 10, 2)
                },
                new PersonDTO
                {
                    Id = 2,
                    FirstName = "Indira",
                    LastName = "Gandhi",
                    Address = "Allahabad - India",
                    Gender = "Female",
                    BirthDay = new DateTime(1917, 11, 19)
                }
            };

            // Act
            var personList = _converter.ParseList(dtoList);

            // Assert
            personList.Should().NotBeNull();
            personList.Should().HaveCount(2);

            personList[0].Should().BeEquivalentTo(new Person
            {
                Id = 1,
                FirstName = "Mahatma",
                LastName = "Gandhi",
                Address = "Porbandar - India",
                Gender = "Male",
                //BirthDay = new DateTime(1869, 10, 2)
            });
            personList[1].Should().BeEquivalentTo(new Person {
                Id = 2,
                FirstName = "Indira",
                LastName = "Gandhi",
                Address = "Allahabad - India",
                Gender = "Female"
            });

            personList[0].FirstName.Should().Be("Mahatma");
            personList[1].FirstName.Should().Be("Indira");
            personList[1].LastName.Should().Be("Gandhi");
        }

        [Fact]
        public void Parse_NullListPersonDTOShouldReturnNull()
        {
            List<PersonDTO> dto = null;
            var listPerson = _converter.ParseList(dto);
            listPerson.Should().BeNull();
        }

        [Fact]
        public void ParseList_ShouldConvertPersonListToPersonDTOList()
        {
            // Arrange
            var dtoList = new List<Person>
            {
                new Person
                {
                    Id = 1,
                    FirstName = "Mahatma",
                    LastName = "Gandhi",
                    Address = "Porbandar - India",
                    Gender = "Male",
                    //BirthDay = new DateTime(1869, 10, 2)
                },
                new Person
                {
                    Id = 2,
                    FirstName = "Indira",
                    LastName = "Gandhi",
                    Address = "Allahabad - India",
                    Gender = "Female",
                    //BirthDay = new DateTime(1917, 11, 19)
                }
            };

            // Act
            var personList = _converter.ParseList(dtoList);

            // Assert
            personList.Should().NotBeNull();
            personList.Should().HaveCount(2);

            personList[0].Should().BeEquivalentTo(new PersonDTO
            {
                Id = 1,
                FirstName = "Mahatma",
                LastName = "Gandhi",
                Address = "Porbandar - India",
                Gender = "Male",
                BirthDay = new DateTime(1869, 10, 2)
            }, options => options.Excluding(person => person.BirthDay));

            personList[1].Should().BeEquivalentTo(new PersonDTO {
                Id = 2,
                FirstName = "Indira",
                LastName = "Gandhi",
                Address = "Allahabad - India",
                Gender = "Female",
                BirthDay = new DateTime(1917, 11, 19)
            }, options => options.Excluding(person => person.BirthDay));

            personList[0].FirstName.Should().Be("Mahatma");
            personList[1].FirstName.Should().Be("Indira");
            personList[1].LastName.Should().Be("Gandhi");
        }

        [Fact]
        public void Parse_NullListPersonShouldReturnNull()
        {
            List<Person> dto = null;
            var listPerson = _converter.ParseList(dto);
            listPerson.Should().BeNull();
        }
    }
}
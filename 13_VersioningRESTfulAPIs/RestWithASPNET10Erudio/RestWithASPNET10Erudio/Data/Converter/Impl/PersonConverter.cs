using RestWithASPNET10Erudio.Data.Converter.Contract;
using RestWithASPNET10Erudio.Data.DTO.V2;
using RestWithASPNET10Erudio.Model;

namespace RestWithASPNET10Erudio.Data.Converter.Impl
{
    public class PersonConverter : IParser<PersonDTO, Person>, IParser<Person, PersonDTO>
    {
        public Person Parse(PersonDTO origin)
        {
            if (origin == null) return null;
            return new Person
            {
                Id = origin.Id,
                FirstName = origin.FirstName,
                LastName = origin.LastName,
                Address = origin.Address,
                Gender = origin.Gender,
                // BirthDay = origin.BirthDay
            };
        }

        public PersonDTO Parse(Person origin)
        {
            if (origin == null) return null;
            return new PersonDTO
            {
                Id = origin.Id,
                FirstName = origin.FirstName,
                LastName = origin.LastName,
                Address = origin.Address,
                Gender = origin.Gender,
                BirthDay = DateTime.Now
                // Mocking a birthday since the Person entity does not have this field.
                //BirthDay = origin.BirthDay ?? DateTime.Now
            };
        }

        public List<Person> ParseList(List<PersonDTO> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }

        public List<PersonDTO> ParseList(List<Person> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }
    }
}

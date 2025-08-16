using RestWithASPNET10Erudio.Model;

namespace RestWithASPNET10Erudio.Services.Impl
{
    public class PersonServicesImpl : IPersonServices
    {
        public Person FindById(long id)
        {
            var person =  MockPerson((int)id);
            return person;
        }
        public List<Person> FindAll()
        {
            List<Person> persons = new List<Person>();
            for (int i = 0; i < 8; i++)
            {
                persons.Add(MockPerson(i));
            }
            return persons;
        }

        public Person Create(Person person)
        {
            person.Id = new Random().Next(1, 1000); // Simulate ID assignment
            return person;
        }

        public Person Update(Person person)
        {
            return person;
        }
        public void Delete(long id)
        {
            // Simulate deletion logic
        }

        private Person MockPerson(int i)
        {
            var person = new Person
            {
                Id = new Random().Next(1, 1000),
                FirstName = "John " + i,
                LastName = "Doe " + i,
                Address = "123 Main Street " + i,
                Gender = "Male"
            }; 
            return person;
        }


    }
}

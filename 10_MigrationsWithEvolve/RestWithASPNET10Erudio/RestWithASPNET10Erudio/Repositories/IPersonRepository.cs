using RestWithASPNET10Erudio.Model;

namespace RestWithASPNET10Erudio.Repositories
{
    public interface IPersonRepository
    {
        Person Create(Person person);

        Person FindById(long id);

        List<Person> FindAll();

        Person Update(Person person);

        void Delete(long id);
    }
}

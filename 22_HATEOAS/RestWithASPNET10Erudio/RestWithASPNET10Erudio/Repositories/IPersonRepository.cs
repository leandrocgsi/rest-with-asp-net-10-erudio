using RestWithASPNET10Erudio.Model;

namespace RestWithASPNET10Erudio.Repositories
{
    public interface IPersonRepository : IRepository<Person>
    {
        Person Disable(long id);
    }
}

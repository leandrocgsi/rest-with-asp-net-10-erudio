using RestWithASPNET10Erudio.Model;

namespace RestWithASPNET10Erudio.Repositories
{
    public interface IPersonRepository : IRepository<Person>
    {
        Person Disable(long id);
        List<Person> FindByName(string firstName, string lastName);

        PagedSearch<Person> FindWithPagedSearch(
            string name,
            string sortDirection,
            int pageSize,
            int page
        );
    }
}

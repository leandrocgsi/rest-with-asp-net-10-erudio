using RestWithASPNET10Erudio.Model;
using RestWithASPNET10Erudio.Model.Context;

namespace RestWithASPNET10Erudio.Repositories.Impl
{
    public class PersonRepository(MSSQLContext context)
        : GenericRepository<Person>(context), IPersonRepository
    {
        public Person Disable(long id)
        {
            var person = _context.Persons.Find(id);
            if (person == null) return null;
            person.Enabled = false;
            _context.SaveChanges();
            return person;
        }

        public List<Person> FindByName(
            string firstName, string lastName)
        {
            var query = _context.Persons.AsQueryable();
            if (!string.IsNullOrWhiteSpace(firstName))
                query = query.Where(p => p.FirstName.Contains(firstName));
            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(p => p.LastName.Contains(lastName));
            // return query.ToList();
            return [.. query];
        }
    }
}

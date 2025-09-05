using Mapster;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Hypermedia.Utils;
using RestWithASPNET10Erudio.Model;
using RestWithASPNET10Erudio.Repositories;

namespace RestWithASPNET10Erudio.Services.Impl
{
    public class PersonServicesImpl : IPersonServices
    {

        private IPersonRepository _repository;
        public PersonServicesImpl(IPersonRepository repository)
        {
            _repository = repository;
        }

        public List<PersonDTO> FindAll()
        {
            return _repository.FindAll().Adapt<List<PersonDTO>>();
        }

        public PersonDTO FindById(long id)
        {
            return _repository.FindById(id).Adapt<PersonDTO>();
        }

        public PersonDTO Create(PersonDTO person)
        {
            var entity = person.Adapt<Person>();
            entity = _repository.Create(entity);
            return entity.Adapt<PersonDTO>();
        }

        public PersonDTO Update(PersonDTO person)
        {
            var entity = person.Adapt<Person>();
            entity = _repository.Update(entity);
            return entity.Adapt<PersonDTO>();
        }
        public void Delete(long id)
        {
            _repository.Delete(id);
        }

        public PersonDTO Disable(long id)
        {
            var entity = _repository.Disable(id);
            return entity.Adapt<PersonDTO>();
        }

        public List<PersonDTO> FindByName(string firstName, string lastName)
        {
            return _repository.FindByName(firstName, lastName)
                .Adapt<List<PersonDTO>>();
        }

        public PagedSearchDTO<PersonDTO> FindWithPagedSearch(
            string name,
            string sortDirection,
            int pageSize,
            int page)
        {
            var (query, countQuery, sort, size, offset) = BuildQueries(
                name, sortDirection, pageSize, page);

            var persons = _repository.FindWithPagedSearch(query);
            var totalResults = _repository.GetCount(countQuery);

            return new PagedSearchDTO<PersonDTO>
            {
                CurrentPage = page,
                List = persons.Adapt<List<PersonDTO>>(),
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }

        private (
            string query,
            string countQuery,
            string sort,
            int size,
            int offset)
            BuildQueries(
                string name,
                string sortDirection,
                int pageSize,
                int page)
        {
            page = Math.Max(1, page);

            var offset = (page - 1) * pageSize;
            var size = pageSize < 1 ? 1 : pageSize;

            var sort = !string.IsNullOrWhiteSpace(sortDirection) &&
                !sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase)
                ? "asc" : "desc";

            var whereClause = $" FROM person p WHERE 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(name))
                whereClause += $" AND (p.first_name LIKE '%{name}%' ";

            var query = $@"
                SELECT * {whereClause}
                ORDER BY p.first_name {sort}
                OFFSET {offset} ROWS FETCH NEXT {size} ROWS ONLY";

            var countQuery = $"SELECT COUNT(*) {whereClause}";
            return (query, countQuery, sort, size, offset);
        }
    }
}

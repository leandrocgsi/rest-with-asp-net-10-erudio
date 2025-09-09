using Microsoft.AspNetCore.Mvc;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Hypermedia.Utils;

namespace RestWithASPNET10Erudio.Services
{
    public interface IPersonServices
    {

        PersonDTO Create(PersonDTO person);

        PersonDTO FindById(long id);

        List<PersonDTO> FindAll();

        PersonDTO Update(PersonDTO person);

        void Delete(long id);

        PersonDTO Disable(long id);

        List<PersonDTO> FindByName(string firstName, string lastName);

        PagedSearchDTO<PersonDTO> FindWithPagedSearch(
            string name,
            string sortDirection,
            int pageSize,
            int page
        );

        Task<List<PersonDTO>> MassCreationAsync(IFormFile file);

        FileContentResult ExportPage(
            int page,
            int pageSize,
            string sortDirection,
            string acceptHeader,
            string name);
    }
}

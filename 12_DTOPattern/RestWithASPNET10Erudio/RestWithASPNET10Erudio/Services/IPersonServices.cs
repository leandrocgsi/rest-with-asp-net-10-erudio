using RestWithASPNET10Erudio.Data.DTO;

namespace RestWithASPNET10Erudio.Services
{
    public interface IPersonServices
    {

        PersonDTO Create(PersonDTO person);

        PersonDTO FindById(long id);

        List<PersonDTO> FindAll();

        PersonDTO Update(PersonDTO person);

        void Delete(long id);
    }
}

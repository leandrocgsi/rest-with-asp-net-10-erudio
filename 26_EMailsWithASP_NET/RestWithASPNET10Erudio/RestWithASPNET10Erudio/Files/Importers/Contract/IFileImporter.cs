using RestWithASPNET10Erudio.Data.DTO.V1;

namespace RestWithASPNET10Erudio.Files.Importers.Contract
{
    public interface IFileImporter
    {
        Task<List<PersonDTO>> ImportFileAsync(Stream fileStream);
    }
}

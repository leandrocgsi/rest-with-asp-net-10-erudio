using Mapster;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Files.Exporters.Factory;
using RestWithASPNET10Erudio.Files.Importers.Factory;
using RestWithASPNET10Erudio.Hypermedia.Utils;
using RestWithASPNET10Erudio.Model;
using RestWithASPNET10Erudio.Repositories;

namespace RestWithASPNET10Erudio.Services.Impl
{
    public class PersonServicesImpl : IPersonServices
    {

        private IPersonRepository _repository;
        private readonly FileImporterFactory _fileImporterFactory;
        private readonly FileExporterFactory _fileExporterFactory;
        private readonly ILogger<PersonServicesImpl> _logger;

        public PersonServicesImpl(
            IPersonRepository repository,
            FileImporterFactory fileImporterFactory,
            FileExporterFactory fileExporterFactory,
            ILogger<PersonServicesImpl> logger
            )
        {
            _repository = repository;
            _fileImporterFactory = fileImporterFactory;
            _fileExporterFactory = fileExporterFactory;
            _logger = logger;
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
            var result = _repository.FindWithPagedSearch(name, sortDirection, pageSize, page);
            return result.Adapt<PagedSearchDTO<PersonDTO>>();
        }

        public async Task<List<PersonDTO>> MassCreationAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogError("File is null or empty.");
                throw new ArgumentException("File is null or empty.");
            }

            using var stream = file.OpenReadStream();
            var fileName = file.FileName;
            try
            {   
                var importer = _fileImporterFactory.GetImporter(fileName);
                var persons = await importer.ImportFileAsync(stream);

                var entities = persons
                    .Select(dto => _repository.Create(
                        dto.Adapt<Person>()))
                        .ToList();
                return entities.Adapt<List<PersonDTO>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during mass creation from file: {FileName}", file.FileName);
                throw;
            }
        }

        public FileContentResult ExportPage(
            int page,
            int pageSize,
            string sortDirection,
            string acceptHeader,
            string name)
        {
            _logger.LogInformation(
                "Exporting page: {page}, {pageSize}, {sortDirection}, {acceptHeader}, {name}",
                page, pageSize, sortDirection, acceptHeader, name);

            var content = FindWithPagedSearch(
                name, sortDirection, pageSize, page);

            try {
                var exporter = _fileExporterFactory.GetExporter(acceptHeader);
                var people = content.List.Adapt<List<PersonDTO>>();
                return exporter.ExportFile(people);
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, "Unsupported export format requested: {AcceptHeader}", acceptHeader);
                throw;
            }

        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Files.Importers.Factory;
using RestWithASPNET10Erudio.Hypermedia.Utils;
using RestWithASPNET10Erudio.Services;

namespace RestWithASPNET10Erudio.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]/v1")]
    [Authorize("Bearer")]
    public class PersonController : ControllerBase
    {
        private IPersonServices _personService;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IPersonServices personService,
            ILogger<PersonController> logger)
        {
            _personService = personService;
            _logger = logger;
        }

        [HttpGet("{sortDirection}/{pageSize}/{page}")]
        [ProducesResponseType(200, Type = typeof (PagedSearchDTO<PersonDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Get(
            [FromQuery] string name,
            string sortDirection,
            int pageSize,
            int page
            )
        {
            _logger.LogInformation(
                "Fetching persons with paged search: {name}, {sortDirection}, {pageSize}, {page}",
                name, sortDirection, pageSize, page);
            return Ok(_personService.FindWithPagedSearch(name, sortDirection, pageSize, page));
        }

        [HttpGet("find-by-name")]
        [ProducesResponseType(200, Type = typeof (List<PersonDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult GetByName(
            [FromQuery] string firstName,
            [FromQuery] string lastName
        )
        {
            _logger.LogInformation(
                "Fetching persons by name: {firstName} {lastName}",
                firstName, lastName);
            return Ok(_personService.FindByName(firstName, lastName));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        // [EnableCors("LocalPolicy")]
        public IActionResult Get(long id)
        {
            _logger.LogInformation("Fetching person with ID {id}", id);
            var person = _personService.FindById(id);
            if (person == null)
            {
                _logger.LogWarning("Person with ID {id} not found", id);
                return NotFound();
            }
            return Ok(person);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        // [EnableCors("MultipleOriginPolicy")]
        public IActionResult Post([FromBody] PersonDTO person)
        {
            _logger.LogInformation("Creating new Person: {firstName}", person.FirstName);

            var createdPerson = _personService.Create(person);
            if (createdPerson == null)
            {
                _logger.LogError("Failed to create person with name {firstName}", person.FirstName);
                return NotFound();
            }
            return Ok(createdPerson);
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Put([FromBody] PersonDTO person)
        {
            _logger.LogInformation("Updating person with ID {id}", person.Id);

            var createdPerson = _personService.Update(person);
            if (createdPerson == null)
            {
                _logger.LogError("Failed to update person with ID {id}", person.Id);
                return NotFound();
            }
            _logger.LogDebug("Person updated successfully: {firstName}", createdPerson.FirstName);
            return Ok(createdPerson);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204, Type = typeof(PersonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Deleting person with ID {id}", id);
            _personService.Delete(id);
            _logger.LogDebug("Person with ID {id} deleted successfully", id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Disable(long id)
        {
            _logger.LogInformation("Disabling person with ID {id}", id);
            var disabledPerson = _personService.Disable(id);
            if (disabledPerson == null)
            {
                _logger.LogError("Failed to disable person with ID {id}", id);
                return NotFound();
            }
            _logger.LogDebug("Person with ID {id} disabled successfully", id);
            return Ok(disabledPerson);
        }

        [HttpPost("massCreation")]
        [ProducesResponseType(200, Type = typeof(List<PersonDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> MassCreation(
            [FromForm] FileUploadDTO input)
        {
            if (input.File == null || input.File.Length == 0)
            {
                _logger.LogWarning("No file uploaded for mass creation");
                return BadRequest("File is Required!");
            }
            _logger.LogInformation("Starting mass creation from uploaded file: {fileName}", input.File.FileName);

            var people = await _personService.MassCreationAsync(input.File);

            if (people == null)
            {
                _logger.LogError("Mass creation failed for file: {fileName}", input.File.FileName);
                return NoContent();
            }
            _logger.LogInformation("Mass creation completed successfully with {count} records", people.Count);
            return Ok(people);
        }

        [HttpGet("exportPage/{sortDirection}/{pageSize}/{page}")]
        [ProducesResponseType(200, Type = typeof(FileContentResult))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(415)]
        [Produces(
            MediaTypes.ApplicationXlsx,
            MediaTypes.ApplicationCsv
        )]
        public IActionResult ExportPage(
            string sortDirection,
            int pageSize,
            int page,
            [FromQuery] string name = ""
        )
        {
            var acceptHeader = Request.Headers["Accept"].ToString();
            if (string.IsNullOrWhiteSpace(acceptHeader))
            {
                return BadRequest("Accept header is required");
            }

            _logger.LogInformation(
                "Exporting persons with paged search: {name}, {sortDirection}, {pageSize}, {page}, {acceptHeader}",
                name, sortDirection, pageSize, page, acceptHeader);

            try
            {
                var fileResult = _personService.ExportPage(
                    page,
                    pageSize,
                    sortDirection,
                    acceptHeader,
                    name);

                return fileResult;
            }
            catch (NotSupportedException ex)
            {
                _logger.LogWarning(ex, "Unsupported export format " +
                    "requested: {AcceptHeader}", acceptHeader);
                return StatusCode(
                    StatusCodes.Status415UnsupportedMediaType, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error " +
                    "while exporting data");

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Internal Server Error");
            }
        }
    }
}

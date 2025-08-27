using Microsoft.AspNetCore.Mvc;
using RestWithASPNET10Erudio.Data.DTO.V2;
using RestWithASPNET10Erudio.Services.Impl;

namespace RestWithASPNET10Erudio.Controllers.V2
{
    [ApiController]
    [Route("api/[controller]/v2")]
    public class PersonController : ControllerBase
    {
        private PersonServicesImplV2 _personService;
        private readonly ILogger<PersonController> _logger;

        public PersonController(PersonServicesImplV2 personService,
            ILogger<PersonController> logger)
        {
            _personService = personService;
            _logger = logger;
        }

        
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
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
    }
}

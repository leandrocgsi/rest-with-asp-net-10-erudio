using HelloDocker.Model;
using HelloDocker.Services;
using Microsoft.AspNetCore.Mvc;

namespace HelloDocker.Controllers
{
    [ApiController]
    [Route("")]
    public class DockerController : ControllerBase
    {
        private readonly ILogger<DockerController> _logger;
        private readonly InstanceInformationService _service;

        public DockerController(ILogger<DockerController> logger, InstanceInformationService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("")]
        public IActionResult ImUpAndRunning()
        {
            return Ok(new { healthy = true });
        }

        [HttpGet("hello-docker")]
        public Model.HelloDocker Greeting()
        {
            _logger.LogInformation("Endpoint /hello-docker is called!!!");

            return new Model.HelloDocker(
                "Hello Docker - V1",
                _service.RetrieveInstanceInfo()
            );
        }
    }
}
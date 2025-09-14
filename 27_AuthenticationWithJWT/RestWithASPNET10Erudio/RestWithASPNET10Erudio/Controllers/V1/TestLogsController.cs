using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestWithASPNET10Erudio.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]/v1")]
    [Authorize("Bearer")]
    public class TestLogsController : ControllerBase
    {

        private readonly ILogger<TestLogsController> _logger;

        public TestLogsController(ILogger<TestLogsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult LogTest()
        {
            _logger.LogTrace("This is a TRACE log message.");
            _logger.LogDebug("This is a DEBUG log message.");
            _logger.LogInformation("This is an INFORMATION log message.");
            _logger.LogWarning("This is a WARNING log message.");
            _logger.LogError("This is an ERROR log message.");
            _logger.LogCritical("This is a CRITICAL log message.");
            return Ok("Log messages have been generated. Check your logging output.");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Services;

namespace RestWithASPNET10Erudio.Controllers.V1
{

    [ApiController]
    [Route("api/[controller]/v1")]
    public class EmailController(
        IEmailService emailService,
        ILogger<EmailController> logger
    ) : ControllerBase
    {
        private readonly IEmailService _emailService = emailService;
        private readonly ILogger<EmailController> _logger = logger;

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult SendEmail(
            [FromBody] EmailRequestDTO emailRequest
        )
        {
            _logger.LogInformation("Sending email to {to}", emailRequest.To);
            _emailService.SendSimpleEmail(
                emailRequest.To,
                emailRequest.Subject,
                emailRequest.Body
            );
            return Ok("Email sent successfully");
        }
    }
}

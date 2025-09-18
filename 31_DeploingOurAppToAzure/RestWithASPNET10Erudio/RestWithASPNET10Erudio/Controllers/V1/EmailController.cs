using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.Services;
using System.Text.Json;

namespace RestWithASPNET10Erudio.Controllers.V1
{

    [ApiController]
    [Route("api/[controller]/v1")]
    [Authorize("Bearer")]
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
            _emailService.SendSimpleEmail(emailRequest);
            return Ok("Email sent successfully");
        }

        [HttpPost("with-attachment")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SendEmailWithAttachment(
            [FromForm] string emailRequest,
            [FromForm] FileUploadDTO attachment
        )
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            EmailRequestDTO emailRequestDto = JsonSerializer
                .Deserialize<EmailRequestDTO>(emailRequest, options);

            if (emailRequestDto == null)
            {
                _logger.LogWarning("Invalid email request data");
                return BadRequest("Invalid email request data");
            }

            if (attachment?.File == null ||
                attachment?.File.Length == 0)
            {
                _logger.LogWarning("Attachment is null or empty");
                return BadRequest("Attachment is null or empty");
            }

            _logger.LogInformation(
                "Sending email with attachment to {to}",
                emailRequestDto.To);
            
            await _emailService.SendEmailWithAttachment(
                emailRequestDto, attachment.File);
            
            return Ok("Email with attachment sent successfully");
        }

    }
}

using RestWithASPNET10Erudio.Data.DTO.V1;

namespace RestWithASPNET10Erudio.Services
{
    public interface IEmailService
    {
        void SendSimpleEmail(EmailRequestDTO emailRequest);
        Task SendEmailWithAttachment(EmailRequestDTO emailRequest,
            IFormFile attachment);
    }
}

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using RestWithASPNET10Erudio.Mail.Settings;

namespace RestWithASPNET10Erudio.Mail
{
    public class EmailSender(
        EmailSettings settings,
        ILogger<EmailSender> logger)
    {
        private readonly EmailSettings _settings = settings;
        private readonly ILogger<EmailSender> _logger = logger;

        private string _to;
        private string _subject;
        private string _body;
        private readonly List<MailboxAddress> _recipients = new();
        private string? _attachment;

        public EmailSender To(string to)
        {
            _to = to;
            _recipients.Clear();
            _recipients.AddRange(ParseRecipients(to));
            return this;
        }

        public EmailSender WithSubject(string subject)
        {
            _subject = subject;
            return this;
        }

        public EmailSender WithMessage(string body)
        {
            _body = body;
            return this;
        }

        public EmailSender Attach(string filePath)
        {
            if (File.Exists(filePath))
            {
                _attachment = filePath;
                return this;
            }
            else
            {
                _logger.LogWarning("Attachment file not found: {FilePath}", filePath);
            }
            return this;
        }

        public void Send()
        {
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(_settings.From, _settings.Username));

            message.To.AddRange(_recipients);
            message.Subject = _subject ?? _settings.Subject ?? "No Subject";

            var builder = new BodyBuilder
            {
                TextBody = _body ?? _settings.Message ?? ""
            };

            if (!string.IsNullOrWhiteSpace(_attachment))
            {
                var filename = Path.GetFileName(_attachment);
                builder.Attachments
                    .Add(filename, File.ReadAllBytes(_attachment));
            }
            message.Body = builder.ToMessageBody();
            
            try
            {
                using var client = new SmtpClient();

                client.Connect(
                    _settings.Host,
                    _settings.Port,
                    _settings.Ssl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                    
                client.Authenticate(_settings.Username, _settings.Password);
                client.Send(message);
                client.Disconnect(true);

                _logger.LogWarning("E-mail successfully sent to {Recipients}",
                    string.Join(";", _recipients));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send e-mail to {Recipients}",
                    string.Join(";", _recipients));
                throw;
            }
            finally
            {
                Reset();
            }
        }

        private IEnumerable<MailboxAddress> ParseRecipients(string to)
        {
            var tosWithoutSpaces = to.Replace(" ", string.Empty);
            var recipients = tosWithoutSpaces.Split(';',
                StringSplitOptions.RemoveEmptyEntries);

            var list = new List<MailboxAddress>();

            foreach (var address in recipients)
            {
                try
                {
                    var mailbox = MailboxAddress.Parse(address);
                    list.Add(mailbox);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Invalid e-mail address: {Address}", address);
                }
            }
            return list;
        }

        private void Reset()
        {
            _to = null;
            _subject = null;
            _body = null;
            _recipients.Clear();
            _attachment = null;
        }
    }
}

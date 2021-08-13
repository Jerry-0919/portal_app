using diga.web.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace diga.web.Helpers
{
    public class EmailHelper
    {
        private readonly EmailOptions _emailOptions;

        public EmailHelper(IOptions<EmailOptions> emailOptions)
        {
            _emailOptions = emailOptions.Value;
        }

        public async Task SendAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(string.Empty, _emailOptions.Login));
            emailMessage.To.Add(new MailboxAddress(string.Empty, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailOptions.Host, _emailOptions.Port, true);
                await client.AuthenticateAsync(_emailOptions.Login, _emailOptions.Password);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}

using Expenses.Application.Common.Interfaces;
using Expenses.Infrastructure.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace Expenses.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpServerOptions _options;

        public EmailService(IOptions<SmtpServerOptions> options)
        {
            _options = options.Value;
        }

        public async Task SendAsync(string receiver, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.SenderName, _options.SenderEmail));
            message.To.Add(MailboxAddress.Parse(receiver));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_options.Host, _options.Port, false);
                client.Authenticate(_options.Username, _options.Password);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}

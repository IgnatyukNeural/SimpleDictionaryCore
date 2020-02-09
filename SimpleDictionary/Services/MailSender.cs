using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MimeKit;

namespace SimpleDictionary.Services
{
    public class MailSender : IMailSender
    {
        private readonly IConfiguration config;

        public MailSender(IConfiguration config)
        {
            this.config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("SimpleDictionary", "simpledictionary@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (SmtpClient client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587);

                await client.AuthenticateAsync(
                    config.GetValue<string>("AuthCredits:EmailLogin"),
                    config.GetValue<string>("AuthCredits:EmailPassword"));

                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}

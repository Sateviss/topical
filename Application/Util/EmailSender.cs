using System.Threading.Tasks;
using AspNetCore.Email;
using MailKit.Net.Smtp;
using MimeKit;

namespace Application.Util
{
    public class EmailSender : IEmailSender
    {
        private string _host;
        private string _login;
        private string _password;

        public EmailSender(string host, string login, string password)
        {
            _host = host;
            _login = login;
            _password = password;
        }

        public async Task<bool> SendEmailAsync(string recipient, string subject, string body)
        {
            var emailMessage = new MimeMessage();
 
            emailMessage.From.Add(new MailboxAddress("", _login));
            emailMessage.To.Add(new MailboxAddress("", recipient));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };
             
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_host, 25, false);
                await client.AuthenticateAsync(_login, _password);
                await client.SendAsync(emailMessage);
 
                await client.DisconnectAsync(true);
            }
            return true;
        }

        public Task<bool> SendEmailAsync(EmailDto input)
        {
            return SendEmailAsync(input.Recipients, input.Subject, input.Body);
        }
    }
}
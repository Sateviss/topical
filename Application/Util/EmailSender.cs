using System;
using System.Net.Mail;
using System.Threading.Tasks;
using AspNetCore.Email;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using IEmailSender = Microsoft.AspNetCore.Identity.UI.Services.IEmailSender;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Application.Util
{
    public class CustomEmailConfirmationTokenProvider<TUser>
                                           : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public CustomEmailConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider,
            IOptions<EmailConfirmationTokenProviderOptions> options,
            ILogger<DataProtectorTokenProvider<TUser>> logger)
                                              : base(dataProtectionProvider, options, logger)
        {
    
        }
    }
    public class EmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public EmailConfirmationTokenProviderOptions()
        {
            Name = "EmailDataProtectorTokenProvider";
            TokenLifespan = TimeSpan.FromHours(4);
        }
    }

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

        public async Task SendEmailAsync(string recipient, string subject, string body)
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
                await client.ConnectAsync(_host, 465, true);
                await client.AuthenticateAsync(_login, _password);
                await client.SendAsync(emailMessage);
 
                await client.DisconnectAsync(true);
            }
        }

        public Task SendEmailAsync(EmailDto input)
        {
            return SendEmailAsync(input.Recipients, input.Subject, input.Body);
        }
    }
}
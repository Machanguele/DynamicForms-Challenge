using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Mailer
{
    public class EmailService 
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task SendEmailAsync(List<string> emails, string subject, string message)
        {
            await Execute(_configuration["SendGrid:ApiKey"], subject, message, emails);
        }
        
        public Task Execute(string apiKey, string subject, string message, List<string> emails)
        {
            var client = new SendGridClient(apiKey);
            
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_configuration["SendGrid:SenderEmail"], _configuration["SendGrid:SenderName"]),

                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            foreach (var email in emails)
            {
                msg.AddTo(new EmailAddress(email));
            }

            Task response = client.SendEmailAsync(msg);
            return response;
        }
    }
    
}
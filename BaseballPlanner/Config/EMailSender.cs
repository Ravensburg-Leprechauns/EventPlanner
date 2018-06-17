﻿using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Planner.Config
{
    public class EMailSender
    {
        public AuthMessageSenderOptions _options { get; }

        public EMailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public Task SendEmail(string email, string subject, string htmlContent)
        {
            var client = new SendGridClient(_options.SendGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_options.SenderMail, _options.SenderName),
                Subject = subject,
                PlainTextContent = htmlContent,
                HtmlContent = htmlContent
            };
            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }
    }
}

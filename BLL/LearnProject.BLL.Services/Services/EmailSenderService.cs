using LearnProject.BLL.Contracts;
using LearnProject.BLL.Contracts.Models;
using LearnProject.BLL.Contracts.Models.EmailMessage;
using LearnProject.Shared.Common.Settings;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnProject.BLL.Services.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        readonly EmailSettings settings;

        public EmailSenderService(EmailSettings settings)
        {
            this.settings = settings;
        }

        public async Task<ServiceResponse<int>> SendEmailAsync(string to, string subject, string body)
        {
            var emailMessage = CreateEmailMessage(new EmailMessage() { To = to, Subject = subject, Body = body});

            try
            {
                await SendAsync(emailMessage);
                return ServiceResponse<int>.CreateSuccessfulResponse();
            } 
            catch (Exception ex)
            {
                return ServiceResponse<int>.CreateFailedResponse(ex.Message);
            }
        }

        MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", settings.Username));
            emailMessage.To.Add(new MailboxAddress("email", message.To));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Body };

            return emailMessage;
        }

        async Task SendAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(settings.SmtpServer, settings.Port, true);
                    await client.AuthenticateAsync(settings.Username, settings.Password);
                    await client.SendAsync(message);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}

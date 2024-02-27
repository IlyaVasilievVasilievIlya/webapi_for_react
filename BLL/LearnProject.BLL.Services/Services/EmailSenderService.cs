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

        public Task<ServiceResponse<int>> SendConfirmationEmailAsync(string to, string subject, string body)
        {
            var emailMessage = CreateConfirmationEmailMessage(new EmailMessage() { To = to, Subject = subject, Body = body});
            return TrySend(emailMessage);
        }

        public Task<ServiceResponse<int>> SendPasswordResetEmailAsync(string to, string subject, string body)
        {
            var emailMessage = CreateResetEmailMessage(new EmailMessage() { To = to, Subject = subject, Body = body });
            return TrySend(emailMessage);
        }

        async Task<ServiceResponse<int>> TrySend(MimeMessage emailMessage)
        {
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

        MimeMessage CreateConfirmationEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", settings.Username));
            emailMessage.To.Add(new MailboxAddress("email", message.To));
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = $"<a>{message.Body}</a>";
            bodyBuilder.TextBody = "Confirm your email via link";

            emailMessage.Body = new BodyBuilder().ToMessageBody();

            return emailMessage;
        }

        MimeMessage CreateResetEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", settings.Username));
            emailMessage.To.Add(new MailboxAddress("email", message.To));
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = $"<a>{message.Body}</a>";
            bodyBuilder.TextBody = "Confirm your email via link";

            emailMessage.Body = new BodyBuilder().ToMessageBody();

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

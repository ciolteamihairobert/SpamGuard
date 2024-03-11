using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;
using SpamDetector.Features.UserManagement.Register.Dtos;
using SpamDetector.Models.UserManagement;
using MediatR;

namespace SpamDetector.HelpfulServices.EmailSenderService
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        private readonly MimeMessage _email = new MimeMessage();
        private readonly IMediator _mediator;
        public EmailSenderService(IConfiguration configuration, IMediator mediator)
        {
            _configuration = configuration;
            _mediator = mediator;
        }

        public bool SendWelcomeEmail(UserRegisterDto user)
        {
            try
            {
                CreateWelcomeMailStructure(user);
                SendEmail();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SendResetPasswordEmail(UserPasswordReset user)
        {
            try
            {
                CreateResetPasswordMailStructure(user);
                SendEmail();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendForgotPasswordEmail(string userEmail)
        {
            try
            {
                await CreateForgotPasswordMailStructure(userEmail);
                SendEmail();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SendEmail()
        {
            using (var smtp = new SmtpClient())
            {
                smtp.Connect(_configuration.GetSection("SenderService:EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration.GetSection("SenderService:EmailUserName").Value, _configuration.GetSection("SenderService:EmailPassword").Value);
                smtp.Send(_email);
                smtp.Disconnect(true);
            }
        }

        private void CreateWelcomeMailStructure(UserRegisterDto user)
        {
            var emailBody = new EmailBody(user, null, _mediator);
            ConfigureEmail(user.Email, "Welcome to Spam Guard!", emailBody.GetWelcomeEmailBody());
        }

        private void CreateResetPasswordMailStructure(UserPasswordReset user)
        {
            var emailBody = new EmailBody(null, user, _mediator);
            ConfigureEmail(user.Email, "The account credentials have been updated Spam Guard!", emailBody.GetResetPasswordBody());
        }

        private async Task CreateForgotPasswordMailStructure(string userEmail)
        {
            var emailBody = new EmailBody(null, null, _mediator);
            var body = await emailBody.GetForgotPasswordBody(userEmail);
            ConfigureEmail(userEmail, "Did you forget your password?", body);
        }

        private void ConfigureEmail(string to, string subject, string body)
        {
            _email.From.Add(MailboxAddress.Parse(_configuration.GetSection("SenderService:EmailUserName").Value));
            _email.To.Add(MailboxAddress.Parse(to));
            _email.Subject = subject;
            _email.Body = new TextPart(TextFormat.Html) { Text = body };
        }
    }
}

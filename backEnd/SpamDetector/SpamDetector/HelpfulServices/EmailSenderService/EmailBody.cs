using MediatR;
using Microsoft.AspNetCore.DataProtection;
using MimeKit;
using SpamDetector.Features.UserManagement.Register.Dtos;
using SpamDetector.Features.UserManagement.ResetPassword.Queries.GetToken;
using SpamDetector.Models.UserManagement;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;

namespace SpamDetector.HelpfulServices.EmailSenderService
{
    public class EmailBody
    {
        private readonly UserRegisterDto userRegisterDto;
        private readonly UserPasswordReset userPasswordReset;
        private readonly IMediator _mediator;
        public EmailBody(UserRegisterDto userRegisterDto, UserPasswordReset userPasswordReset, IMediator mediator)
        {
            this.userRegisterDto = userRegisterDto;
            this.userPasswordReset = userPasswordReset;
            _mediator = mediator;
        }
        #region EmailBodies
        private const string WELCOMEBODY = @"
        <html>
            <body>
                <h2><b>Welcome, {0} {1}!</b></h2>
                <p><b>Your account has been created.<b><br>
                <b>The credentials are:<b><br>
                    <b>UserName: {2} <b><br>
                    <b>Password: {3} <b><br><br>
                If you have any questions, feel free and reach out to us via spam.guard.co@gmail.com <br><br>
                Sincerely,</p>
                <p><b>Spam Guard</b></p>
            </body>
        </html>";

        private const string RESETPASSWORDBODY = @"
        <html>
            <body>
                <h2><b>Your password has been updated.<b></h2>
                <p><b>The credentials are:<b><br>
                    <b>UserName: {0} <b><br>
                    <b>Password: {1} <b><br><br>
                If you have any questions, feel free and reach out to us via spam.guard.co@gmail.com <br><br>
                Sincerely,</p>
                <p><b>Spam Guard</b></p>
            </body>
        </html>";

        private const string FORGOTPASSWORDBODY = @"
        <html>
            <body>
                <h2><b>Need to reset your password?<b></h2>
                <p><b>Use your secret code!<b><br> 
                <b>Code: {0}<b><br><br>
                If you did not forget your password, you can ignore this email.<br><br>
                If you have any questions, feel free and reach out to us via spam.guard.co@gmail.com<br><br>
                Sincerely,</p>
                <p><b>Spam Guard</b></p>
            </body>
        </html>";
        #endregion
        public string GetWelcomeEmailBody()
        {
            return string.Format(WELCOMEBODY, userRegisterDto.FirstName, userRegisterDto.LastName, userRegisterDto.Email, userRegisterDto.Password);
        }

        public string GetResetPasswordBody()
        {
            return string.Format(RESETPASSWORDBODY, userPasswordReset.Email, userPasswordReset.Password);
        }

        public async Task<string> GetForgotPasswordBody(string userEmail)
        {
            var resetCode = await _mediator.Send(new GetPasswordResetTokenQuery() { UserEmail = userEmail });
            return string.Format(FORGOTPASSWORDBODY, resetCode);
        }
    }
}

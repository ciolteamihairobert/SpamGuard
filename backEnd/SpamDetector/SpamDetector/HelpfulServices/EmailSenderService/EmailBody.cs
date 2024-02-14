using MimeKit;
using SpamDetector.Features.UserManagement.Register.Dtos;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.HelpfulServices.EmailSenderService
{
    public class EmailBody
    {
        private readonly UserRegisterDto userRegisterDto;
        private readonly UserPasswordReset userPasswordReset;
        public EmailBody(UserRegisterDto userRegisterDto, UserPasswordReset userPasswordReset)
        {
            this.userRegisterDto = userRegisterDto;
            this.userPasswordReset = userPasswordReset;
        }
        #region EmailBodies
        private const string WELCOMEBODY = @"
        <html>
            <body>
                <h2><b>Welcome, {0} {1}!</b></h2>
                <p>Your account has been created.</p>
                <p>The credentials are: <br>
                    UserName: {2} <br>
                    Password: {3} </p>
                <p>If you have any questions, feel free and reach out to spam.guard.co@gmail.com </p>
                Sincerely,
                <p><b>Spam Guard</b></p>
            </body>
        </html>";

        private const string RESETPASSWORDBODY = @"
        <html>
            <body>
                <p>Your password has been updated.</p>
                <p>The credentials are: <br>
                    UserName: {1} <br>
                    Password: {2} </p>
                <p>If you have any questions, feel free and reach out to spam.guard.co@gmail.com </p>
                Sincerely,
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
    }
}

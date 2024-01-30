using MimeKit;
using SpamDetector.Features.UserManagement.Register.Dtos;

namespace SpamDetector.HelpfulServices.EmailSenderService
{
    public class EmailBody
    {
        private readonly UserRegisterDto user;
        public EmailBody(UserRegisterDto user)
        {
            this.user = user;
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
                <p>If you have any questions, feel free to reach out to spam.guard.co@gmail.com </p>
                Sincerely,
                <p><b>Spam Guard</b></p>
            </body>
        </html>";
        #endregion
        public string GetWelcomeEmailBody()
        {
            return string.Format(WELCOMEBODY, user.FirstName, user.LastName, user.Email, user.Password);
        }
    }
}

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SpamDetector.Features.UserManagement.Register.Dtos;

namespace SpamDetector.HelpfulServices.EmailSenderService
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly MimeMessage _email = new MimeMessage();
        private readonly IConfiguration _configuration;
        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool SendEmail(UserRegisterDto user)
        {
            try
            {
                CreateMailStructure(user);
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(_configuration.GetSection("SenderService:EmailHost").Value, 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_configuration.GetSection("SenderService:EmailUserName").Value, _configuration.GetSection("SenderService:EmailPassword").Value);
                    smtp.Send(_email);
                    smtp.Disconnect(true);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void CreateMailStructure(UserRegisterDto user)
        {
            _email.From.Add(MailboxAddress.Parse(_configuration.GetSection("SenderService:EmailUserName").Value));
            _email.To.Add(MailboxAddress.Parse(user.Email));
            _email.Subject = "Welcome to Spam Guard!";
            _email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"
                      <html>
                          <body>
                              <h2><b>Welcome, {user.FirstName} {user.LastName}!</b></h2>
                              <p>Your account has been created.</p>
                              <p>The credentials are: <br>
                               UserName: {user.Email} <br>
                               Password: {user.Password} </p>
                              <p>If you have any questions fell free and reach out to spam.guard.co@gmail.com </p>
                              Sincerely,
                              <p><b>Spam Guard</b></p>
                          </body>
                      </html>"
            };   
        }
    }
}

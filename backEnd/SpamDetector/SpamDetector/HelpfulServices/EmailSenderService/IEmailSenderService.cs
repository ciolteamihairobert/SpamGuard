using SpamDetector.Features.UserManagement.Register.Dtos;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.HelpfulServices.EmailSenderService
{
    public interface IEmailSenderService
    {
        bool SendWelcomeEmail(UserRegisterDto user);
        bool SendResetPasswordEmail(UserPasswordReset user);
        Task<bool> SendForgotPasswordEmail(string userEmail);
    }
}

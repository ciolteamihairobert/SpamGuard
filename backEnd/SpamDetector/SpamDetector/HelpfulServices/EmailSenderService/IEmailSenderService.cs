using SpamDetector.Features.UserManagement.Register.Dtos;

namespace SpamDetector.HelpfulServices.EmailSenderService
{
    public interface IEmailSenderService
    {
        bool SendEmail(UserRegisterDto user);
        void CreateMailStructure(UserRegisterDto user);
    }
}

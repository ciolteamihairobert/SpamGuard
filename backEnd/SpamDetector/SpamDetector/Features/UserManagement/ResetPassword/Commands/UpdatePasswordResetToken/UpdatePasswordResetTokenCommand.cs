using MediatR;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.ResetPassword.Commands.UpdatePasswordResetToken
{
    public class UpdatePasswordResetTokenCommand : IRequest<PasswordResetToken>
    {
        public User User { get; set; }
    }
}

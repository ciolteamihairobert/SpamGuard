using MediatR;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.ResetPassword.Commands.AddPasswordResetToken
{
    public class AddPasswordResetTokenCommand : IRequest
    {
        public string Email { get; set; }
    }
}

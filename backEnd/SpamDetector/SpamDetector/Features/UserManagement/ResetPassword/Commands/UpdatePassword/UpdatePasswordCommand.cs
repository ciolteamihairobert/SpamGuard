using MediatR;
using SpamDetector.Features.UserManagement.Register.Dtos;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.ResetPassword.Commands.UpdatePassword
{
    public class UpdatePasswordCommand : IRequest
    {
        public UserPasswordReset User { get; set; }
    }
}

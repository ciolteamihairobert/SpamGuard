using MediatR;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.Login.Commands.DeleteRefreshToken
{
    public class DeleteRefreshTokenCommand : IRequest
    {
        public User User { get; set; }
    }
}

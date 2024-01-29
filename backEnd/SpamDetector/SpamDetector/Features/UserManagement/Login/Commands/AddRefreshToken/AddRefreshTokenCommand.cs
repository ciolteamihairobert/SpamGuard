using MediatR;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.Login.Commands.AddRefreshToken
{
    public class AddRefreshTokenCommand : IRequest<RefreshToken>
    {
        public User User { get; set; }
    }
}

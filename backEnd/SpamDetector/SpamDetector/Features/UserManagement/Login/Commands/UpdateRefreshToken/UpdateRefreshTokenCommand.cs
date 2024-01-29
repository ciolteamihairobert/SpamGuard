using MediatR;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.Login.Commands.UpdateRefreshToken
{
    public class UpdateRefreshTokenCommand : IRequest<RefreshToken>
    {
        public User User { get; set; }
    }
}

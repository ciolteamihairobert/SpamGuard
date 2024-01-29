using MediatR;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.Login.Commands.UpdateRefreshTokenByUser
{
    public class UpdateRefreshTokenByUserCommand : IRequest
    {
        public UserLogin User { get; set; }
    }
}

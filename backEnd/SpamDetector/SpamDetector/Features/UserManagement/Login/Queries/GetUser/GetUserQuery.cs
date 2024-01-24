using MediatR;
using SpamDetector.Features.UserManagement.Login.Dtos;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.Login.Queries.GetUser
{
    public class GetUserQuery : IRequest<string>
    {
        public UserLoginDto User { get; set; }
    }
}

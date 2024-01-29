using MediatR;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.Login.Queries.GetUser
{
    public class GetUserQuery : IRequest<string>
    {
        public UserLogin User { get; set; }
    }
}

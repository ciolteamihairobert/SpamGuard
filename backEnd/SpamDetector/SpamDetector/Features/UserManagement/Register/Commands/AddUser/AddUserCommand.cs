using MediatR;
using SpamDetector.Features.UserManagement.Register.Dtos;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.Register.Commands.AddUser
{
    public class AddUserCommand : IRequest<User>
    {
        public UserRegisterDto NewUser { get; set; }
    }
}

using MediatR;
using Microsoft.EntityFrameworkCore;
using SpamDetector.Data;
using SpamDetector.HelpfulServices;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.Register.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, User>
    {
        private readonly DataContext _dataContext;
        private readonly AuthService _authService;
        public AddUserCommandHandler(DataContext dataContext, AuthService authService)
        {
            _dataContext = dataContext;
            _authService = authService;
        }
        public async Task<User> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var isAlreadyInDb = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == request.NewUser.Email);
            if (isAlreadyInDb is not null)
            {
                throw new Exception($"The user with username: {request.NewUser.Email} already exists");
            }

            if(_authService.ValidatePassword(request.NewUser.Password))
            {
                _authService.CreatePasswordHash(request.NewUser.Password, out byte[] passwordHash, out byte[] passwordSalt);
                User user = (User)request.NewUser;

                user.PassWordHash = passwordHash;
                user.PassWordSalt = passwordSalt;

                await _dataContext.Users.AddAsync(user, cancellationToken);
                await _dataContext.SaveChangesAsync(cancellationToken);

                return user;
            }

            return null;
        }
    }
}

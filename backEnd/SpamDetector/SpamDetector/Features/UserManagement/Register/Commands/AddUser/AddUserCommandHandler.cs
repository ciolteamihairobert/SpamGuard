using MediatR;
using Microsoft.EntityFrameworkCore;
using SpamDetector.Data;
using SpamDetector.HelpfulServices.AuthenticationService;
using SpamDetector.HelpfulServices.EmailSenderService;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.Register.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, User>
    {
        private readonly DataContext _dataContext;
        private readonly AuthService _authService;
        private readonly IEmailSenderService _emailSenderService;
        public AddUserCommandHandler(DataContext dataContext, AuthService authService, IEmailSenderService emailSenderService)
        {
            _dataContext = dataContext;
            _authService = authService;
            _emailSenderService = emailSenderService;
        }
        public async Task<User> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var isAlreadyInDb = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == request.NewUser.Email);
            if (isAlreadyInDb is not null)
            {
                throw new Exception($"The user with username: {request.NewUser.Email} already exists");
            }

            if(_authService.ValidatePassword(request.NewUser.Password) && _authService.ValidateEmail(request.NewUser.Email))
            {
                _authService.CreatePasswordHash(request.NewUser.Password, out byte[] passwordHash, out byte[] passwordSalt);
                User user = (User)request.NewUser;

                user.PassWordHash = passwordHash;
                user.PassWordSalt = passwordSalt;

                await _dataContext.Users.AddAsync(user, cancellationToken);
                await _dataContext.SaveChangesAsync(cancellationToken);

                if (_emailSenderService.SendEmail(request.NewUser))
                {
                    return user;
                }
            }

            return null;
        }
    }
}

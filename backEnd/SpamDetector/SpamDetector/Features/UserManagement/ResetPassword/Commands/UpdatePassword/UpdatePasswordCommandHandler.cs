using MediatR;
using Microsoft.EntityFrameworkCore;
using SpamDetector.Data;
using SpamDetector.HelpfulServices.AuthenticationService;
using SpamDetector.HelpfulServices.EmailSenderService;

namespace SpamDetector.Features.UserManagement.ResetPassword.Commands.UpdatePassword
{
    public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand>
    {
        private readonly DataContext _dataContext;
        private readonly AuthService _authService;
        private readonly IEmailSenderService _emailSenderService;

        public UpdatePasswordCommandHandler(DataContext dataContext, AuthService authService, IEmailSenderService emailSenderService)
        {
            _dataContext = dataContext;
            _authService = authService;
            _emailSenderService = emailSenderService;
        }
        public async Task Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var isAlreadyInDb = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == request.User.Email);
            if (isAlreadyInDb is null)
            {
                throw new Exception($"The account with username: {request.User.Email} does not exist.");
            }

            var isTokenAlreadyInDb = await _dataContext.PasswordResetTokens.FirstOrDefaultAsync(p => p.UserEmail == request.User.Email && p.Token == request.User.ResetCode);
            if (isTokenAlreadyInDb is not null)
            {
                if (isTokenAlreadyInDb.ExpirationDate >= DateTime.Now)
                {
                    if (request.User.Password.Equals(request.User.PasswordConfirmation) && _authService.ValidatePassword(request.User.Password))
                    {
                        _authService.CreatePasswordHash(request.User.Password, out byte[] passwordHash, out byte[] passwordSalt);

                        isAlreadyInDb.PassWordHash = passwordHash;
                        isAlreadyInDb.PassWordSalt = passwordSalt;

                        _dataContext.Users.Update(isAlreadyInDb);
                        await _dataContext.SaveChangesAsync(cancellationToken);
                        _emailSenderService.SendResetPasswordEmail(request.User);
                    }
                    else
                    {
                        throw new Exception("Make sure the password and confiramtion password are the same and correct!");
                    }
                }
                else
                {
                    throw new Exception("The operation cannot be performed!"); 
                }
            } 
        }
    }
}

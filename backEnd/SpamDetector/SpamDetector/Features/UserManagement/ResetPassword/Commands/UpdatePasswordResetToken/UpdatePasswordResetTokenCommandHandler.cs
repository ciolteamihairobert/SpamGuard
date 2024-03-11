using MediatR;
using Microsoft.EntityFrameworkCore;
using SpamDetector.Data;
using SpamDetector.HelpfulServices.AuthenticationService;
using SpamDetector.HelpfulServices.EmailSenderService;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.ResetPassword.Commands.UpdatePasswordResetToken
{
    public class UpdatePasswordResetTokenCommandHandler : IRequestHandler<UpdatePasswordResetTokenCommand, PasswordResetToken>
    {
        private readonly DataContext _dataContext;
        private readonly AuthService _authService;
        private readonly IEmailSenderService _emailSenderService;

        public UpdatePasswordResetTokenCommandHandler(DataContext dataContext, AuthService authService, IEmailSenderService emailSenderService)
        {
            _dataContext = dataContext;
            _authService = authService;
            _emailSenderService = emailSenderService;
        }

        public async Task<PasswordResetToken> Handle(UpdatePasswordResetTokenCommand request, CancellationToken cancellationToken)
        {
            var isTokenInDb = await _dataContext.PasswordResetTokens.FirstOrDefaultAsync(rt => rt.UserEmail == request.User.Email, cancellationToken);
            if (isTokenInDb is null)
            {
                throw new Exception($"The token does not exist.");
            }

            var newPasswordResetToken = _authService.GetPasswordResetToken(request.User);
            isTokenInDb.Token = newPasswordResetToken.Token;
            isTokenInDb.ExpirationDate = newPasswordResetToken.ExpirationDate;

            _dataContext.PasswordResetTokens.Update(isTokenInDb);
            await _dataContext.SaveChangesAsync(cancellationToken);

            await _emailSenderService.SendForgotPasswordEmail(request.User.Email);

            return newPasswordResetToken;
        }
    }
}

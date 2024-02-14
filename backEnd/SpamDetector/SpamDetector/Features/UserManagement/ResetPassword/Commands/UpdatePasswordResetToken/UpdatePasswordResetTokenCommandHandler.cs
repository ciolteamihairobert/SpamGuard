using MediatR;
using Microsoft.EntityFrameworkCore;
using SpamDetector.Data;
using SpamDetector.HelpfulServices.AuthenticationService;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.ResetPassword.Commands.UpdatePasswordResetToken
{
    public class UpdatePasswordResetTokenCommandHandler : IRequestHandler<UpdatePasswordResetTokenCommand, PasswordResetToken>
    {
        private readonly DataContext _dataContext;
        private readonly AuthService _authService;
        public UpdatePasswordResetTokenCommandHandler(DataContext dataContext, AuthService authService)
        {
            _dataContext = dataContext;
            _authService = authService;
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

            return newPasswordResetToken;
        }
    }
}

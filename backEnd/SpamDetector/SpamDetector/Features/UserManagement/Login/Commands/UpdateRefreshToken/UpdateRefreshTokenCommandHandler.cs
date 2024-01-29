using MediatR;
using Microsoft.EntityFrameworkCore;
using SpamDetector.Data;
using SpamDetector.HelpfulServices;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.Login.Commands.UpdateRefreshToken
{
    public class UpdateRefreshTokenCommandHandler : IRequestHandler<UpdateRefreshTokenCommand, RefreshToken>
    {
        private readonly DataContext _dataContext;
        private readonly AuthService _authService;
        public UpdateRefreshTokenCommandHandler(DataContext dataContext, AuthService authService)
        {
            _dataContext = dataContext;
            _authService = authService;
        }

        public async Task<RefreshToken> Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var isTokenInDb = await _dataContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserEmail == request.User.Email, cancellationToken);
            if (isTokenInDb is null)
            {
                throw new Exception($"The token {request.User.RefreshToken} does not exist.");
            }

            var newRefreshToken = _authService.GenerateRefreshToken(request.User);
            isTokenInDb.Token = newRefreshToken.Token;
            isTokenInDb.User = newRefreshToken.User;
            isTokenInDb.UserEmail = newRefreshToken.UserEmail;
            isTokenInDb.CreationDate = newRefreshToken.CreationDate;
            isTokenInDb.ExpirationDate = newRefreshToken.ExpirationDate;

            _dataContext.RefreshTokens.Update(isTokenInDb);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return newRefreshToken;
        }
    }
}

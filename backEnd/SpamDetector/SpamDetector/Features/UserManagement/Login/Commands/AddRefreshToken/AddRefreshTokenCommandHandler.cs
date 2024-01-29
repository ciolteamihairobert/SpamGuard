using MediatR;
using Microsoft.EntityFrameworkCore;
using SpamDetector.Data;
using SpamDetector.HelpfulServices;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.Login.Commands.AddRefreshToken
{
    public class AddRefreshTokenCommandHandler : IRequestHandler<AddRefreshTokenCommand, RefreshToken>
    {
        private readonly DataContext _dataContext;
        private readonly AuthService _authService;
        public AddRefreshTokenCommandHandler(DataContext dataContext, AuthService authService)
        {
            _dataContext = dataContext;
            _authService = authService;
        }

        public async Task<RefreshToken> Handle(AddRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var isUserInDb = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == request.User.Email, cancellationToken);
            if (isUserInDb is null)
            {
                throw new Exception($"The user with username: {request.User.Email} does not exist.");
            }

            RefreshToken refreshToken = _authService.GenerateRefreshToken(isUserInDb);
            refreshToken.UserEmail = isUserInDb.Email;

            await _dataContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return refreshToken;
        }
    }
}

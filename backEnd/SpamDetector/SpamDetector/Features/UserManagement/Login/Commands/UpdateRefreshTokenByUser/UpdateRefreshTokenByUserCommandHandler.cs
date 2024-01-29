using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SpamDetector.Data;
using SpamDetector.Features.UserManagement.Login.Commands.UpdateRefreshToken;
using SpamDetector.Features.UserManagement.Login.Commands.UpdateRefreshTokenByUser;
using SpamDetector.HelpfulServices.AuthenticationService;

namespace SpamDetector.Features.UserManagement.Login.Commands.UpdateRTByUser
{
    public class UpdateRefreshTokenByUserCommandHandler : IRequestHandler<UpdateRefreshTokenByUserCommand>
    {
        private readonly DataContext _dataContext;
        private readonly AuthService _authService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UpdateRefreshTokenByUserCommandHandler(DataContext dataContext, AuthService authService, IMediator mediatR, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _authService = authService;
            _mediator = mediatR;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(UpdateRefreshTokenByUserCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

            var userFromDb = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == request.User.Email, cancellationToken);

            var isTokenInDb = await _dataContext.RefreshTokens.FirstOrDefaultAsync(
                rt => rt.User.Email == userFromDb.Email, cancellationToken);
            if (isTokenInDb is null)
            {
                throw new Exception($"The token {userFromDb.RefreshToken.Token} does not exist.");
            }

            if (!isTokenInDb.Token.Equals(refreshToken))
            {
                throw new Exception("Invalid Refresh Token.");
            }
            else if(isTokenInDb.ExpirationDate < DateTime.Now)
            {
                throw new Exception("Token expired.");
            }
            
            var newRefreshToken= await _mediator.Send(new UpdateRefreshTokenCommand() { User = userFromDb });
            _authService.SetRefreshToken(newRefreshToken);
        }
    }
}

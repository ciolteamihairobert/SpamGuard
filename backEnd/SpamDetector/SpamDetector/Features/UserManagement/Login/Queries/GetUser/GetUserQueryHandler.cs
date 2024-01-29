using MediatR;
using Microsoft.EntityFrameworkCore;
using SpamDetector.Data;
using SpamDetector.Features.UserManagement.Login.Commands.AddRefreshToken;
using SpamDetector.Features.UserManagement.Login.Commands.DeleteRefreshToken;
using SpamDetector.HelpfulServices;

namespace SpamDetector.Features.UserManagement.Login.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, string>
    {
        private readonly DataContext _dataContext;
        private readonly AuthService _authService;
        private readonly IMediator _mediator;
        public GetUserQueryHandler(DataContext dataContext, AuthService authService, IMediator mediatR)
        {
            _dataContext = dataContext;
            _authService = authService;
            _mediator = mediatR;
        }

        public async Task<string> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var isAlreadyInDb = await _dataContext.Users
                .Where(u => u.Email == request.User.Email)
                .FirstOrDefaultAsync(cancellationToken);

            if (isAlreadyInDb is null)
            {
                throw new Exception($"The user with username: {request.User.Email} does not exist.");
            }

            if (!_authService.VerifyPasswordHash(request.User.Password, isAlreadyInDb.PassWordHash, isAlreadyInDb.PassWordSalt))
            {
                throw new Exception($"The password for the user with username: {request.User.Email} is incorrect.");
            }
            await _mediator.Send(new DeleteRefreshTokenCommand() { User =  isAlreadyInDb });

            var refreshToken =  await _mediator.Send(new AddRefreshTokenCommand() { User = isAlreadyInDb });
            _authService.SetRefreshToken(refreshToken);

            return _authService.CreateToken(isAlreadyInDb);
        }
    }
}

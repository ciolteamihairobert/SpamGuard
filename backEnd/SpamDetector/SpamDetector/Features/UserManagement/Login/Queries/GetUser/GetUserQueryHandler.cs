using MediatR;
using Microsoft.EntityFrameworkCore;
using SpamDetector.Data;
using SpamDetector.HelpfulServices;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.Login.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, string>
    {
        private readonly DataContext _dataContext;
        private readonly AuthService _authService;
        public GetUserQueryHandler(DataContext dataContext, AuthService authService)
        {
            _dataContext = dataContext;
            _authService = authService;
        }

        public async Task<string> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var isAlreadyInDb = await _dataContext.Users
                .Where(u => u.UserName == request.User.UserName)
                .FirstOrDefaultAsync(cancellationToken);

            if (isAlreadyInDb is null)
            {
                throw new Exception($"The user with username: {request.User.UserName} does not exist.");
            }

            if (!_authService.VerifyPasswordHash(request.User.Password, isAlreadyInDb.PassWordHash, isAlreadyInDb.PassWordSalt))
            {
                throw new Exception($"The password for the user with username: {request.User.UserName} is incorrect.");
            }

            return _authService.CreateToken(isAlreadyInDb);
        }
    }
}

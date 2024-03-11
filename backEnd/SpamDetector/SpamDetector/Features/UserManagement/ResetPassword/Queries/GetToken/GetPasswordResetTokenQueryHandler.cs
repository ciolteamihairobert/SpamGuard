using MediatR;
using Microsoft.EntityFrameworkCore;
using SpamDetector.Data;
using SpamDetector.HelpfulServices.AuthenticationService;

namespace SpamDetector.Features.UserManagement.ResetPassword.Queries.GetToken
{
    public class GetPasswordResetTokenQueryHandler : IRequestHandler<GetPasswordResetTokenQuery, string>
    {
        private readonly DataContext _dataContext;
        public GetPasswordResetTokenQueryHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<string> Handle(GetPasswordResetTokenQuery request, CancellationToken cancellationToken)
        {
            var isAlreadyInDb = await _dataContext.Users
               .Where(u => u.Email == request.UserEmail)
               .FirstOrDefaultAsync(cancellationToken);

            if (isAlreadyInDb is null)
            {
                throw new Exception($"The user with username: {request.UserEmail} does not exist.");
            }

            var hasToken = await _dataContext.PasswordResetTokens
                .Where(t => t.UserEmail == request.UserEmail)
                .FirstOrDefaultAsync(cancellationToken);

            if(hasToken is null && hasToken.ExpirationDate <= DateTime.Now)
            {
                throw new Exception($"The user with username: {request.UserEmail} does not have a valid password reset token.");
            }

            return hasToken.Token;
        }
    }
}

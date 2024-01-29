using MediatR;
using Microsoft.EntityFrameworkCore;
using SpamDetector.Data;

namespace SpamDetector.Features.UserManagement.Login.Commands.DeleteRefreshToken
{
    public class DeleteRefreshTokenCommandHandler : IRequestHandler<DeleteRefreshTokenCommand>
    {
        private readonly DataContext _dataContext;
        public DeleteRefreshTokenCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Handle(DeleteRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var isTokenInDb = await _dataContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserEmail == request.User.Email, cancellationToken);
            if (isTokenInDb is null)
            {
                return;
            }

            _dataContext.RefreshTokens.Remove(isTokenInDb);
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}

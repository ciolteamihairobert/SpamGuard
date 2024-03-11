using MediatR;
using Microsoft.EntityFrameworkCore;
using SpamDetector.Data;
using SpamDetector.Features.UserManagement.ResetPassword.Commands.UpdatePasswordResetToken;
using SpamDetector.HelpfulServices.AuthenticationService;
using SpamDetector.HelpfulServices.EmailSenderService;
using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.ResetPassword.Commands.AddPasswordResetToken
{
    public class AddPasswordResetTokenCommandHandler : IRequestHandler<AddPasswordResetTokenCommand>
    {
        private readonly DataContext _dataContext;
        private readonly AuthService _authService;
        private readonly IMediator _mediator;
        private readonly IEmailSenderService _emailSenderService;
        public AddPasswordResetTokenCommandHandler(DataContext dataContext, AuthService authService, IMediator mediator, IEmailSenderService emailSenderService)
        {
            _dataContext = dataContext;
            _authService = authService;
            _mediator = mediator;
            _emailSenderService = emailSenderService;
        }

        public async Task Handle(AddPasswordResetTokenCommand request, CancellationToken cancellationToken)
        {
            var isAlreadyInDb = await _dataContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (isAlreadyInDb is null)
            {
                throw new Exception($"The account with username: {request.Email} does not exist.");
            }

            var isTokenAlreadyInDb = await _dataContext.PasswordResetTokens.FirstOrDefaultAsync(p => p.UserEmail == request.Email);
            if(isTokenAlreadyInDb is null)
            {
                var passwordResetToken = _authService.GetPasswordResetToken(isAlreadyInDb);

                await _dataContext.PasswordResetTokens.AddAsync(passwordResetToken, cancellationToken);
                await _dataContext.SaveChangesAsync(cancellationToken);

                await _emailSenderService.SendForgotPasswordEmail(request.Email);
            }
            else
            {
                if (isTokenAlreadyInDb.ExpirationDate >= DateTime.Now)
                {
                    var passwordResetToken = _authService.GetPasswordResetToken(isAlreadyInDb);

                    await _dataContext.PasswordResetTokens.AddAsync(passwordResetToken, cancellationToken);
                    await _dataContext.SaveChangesAsync(cancellationToken);

                    await _emailSenderService.SendForgotPasswordEmail(request.Email);
                }
                else
                {
                    await _mediator.Send(new UpdatePasswordResetTokenCommand() { User = isAlreadyInDb });
                }
            }
        }
    }
}

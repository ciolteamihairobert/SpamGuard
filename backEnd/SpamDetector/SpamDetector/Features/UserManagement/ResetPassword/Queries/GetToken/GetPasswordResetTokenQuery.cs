using MediatR;

namespace SpamDetector.Features.UserManagement.ResetPassword.Queries.GetToken
{
    public class GetPasswordResetTokenQuery : IRequest<string>
    {
        public string UserEmail { get; set; }
    }
}

using SpamDetector.Models.UserManagement;

namespace SpamDetector.HelpfulServices.AuthenticationService
{
    public interface IAuthService
    {
        string CreateToken(User user);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        RefreshToken GenerateRefreshToken(User user);
        void SetRefreshToken(RefreshToken refreshToken);
        PasswordResetToken GetPasswordResetToken(User user);
    }
}

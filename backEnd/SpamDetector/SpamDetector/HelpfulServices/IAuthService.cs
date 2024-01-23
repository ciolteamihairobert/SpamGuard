using SpamDetector.Models.UserManagement;

namespace SpamDetector.HelpfulServices
{
    public interface IAuthService
    {
        string CreateToken(User user);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}

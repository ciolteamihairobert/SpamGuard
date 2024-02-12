using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SpamDetector.Models.UserManagement
{
    public class User
    {
        [Key]
        public string Email { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public byte[] PassWordHash { get; set; }
        public byte[] PassWordSalt { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public PasswordResetToken PasswordResetToken { get; set; }
    }
}

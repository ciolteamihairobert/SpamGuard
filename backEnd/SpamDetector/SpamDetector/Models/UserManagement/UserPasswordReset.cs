using SpamDetector.Features.UserManagement.Register.Dtos;

namespace SpamDetector.Models.UserManagement
{
    public class UserPasswordReset
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }

        public static explicit operator User(UserPasswordReset useerDto)
        {
            return new()
            {
                Email = useerDto.Email,
            };
        }
    }
}

using SpamDetector.Models.UserManagement;

namespace SpamDetector.Features.UserManagement.Register.Dtos
{
    public class UserRegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public static explicit operator User(UserRegisterDto useerDto)
        {
            return new()
            {
                FirstName = useerDto.FirstName,
                LastName = useerDto.LastName,
                Email = useerDto.Email,
                UserName = useerDto.UserName
            };
        }
    }
}

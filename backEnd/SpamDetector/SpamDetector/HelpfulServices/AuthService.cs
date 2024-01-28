using Microsoft.IdentityModel.Tokens;
using SpamDetector.Models.UserManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace SpamDetector.HelpfulServices
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static readonly Regex hasNumber = new Regex(@"[0-9]+");
        private static readonly Regex hasUpperChar = new Regex(@"[A-Z]+");
        private static readonly Regex hasMiniMaxChars = new Regex(@".{8,15}");
        private static readonly Regex hasLowerChar = new Regex(@"[a-z]+");
        private static readonly Regex hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
        public AuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public bool ValidatePassword(string password)
        {
            var valid = true;
            if (string.IsNullOrWhiteSpace(password))
            {
                valid = false;
                throw new Exception("Password should not be empty");
            }
            
            ValidateCondition(password, hasLowerChar, "Password should contain at least one lower case letter.", valid);
            ValidateCondition(password, hasUpperChar, "Password should contain at least one upper case letter.", valid);
            ValidateCondition(password, hasMiniMaxChars, "Password should not be lesser than 8 or greater than 15 characters.", valid);
            ValidateCondition(password, hasNumber, "Password should contain at least one numeric value.", valid);
            ValidateCondition(password, hasSymbols, "Password should contain at least one special case character.", valid);

            return valid;
        }

        private static bool ValidateCondition(string password, Regex regex, string errorMessage, bool valid)
        {
            if (!regex.IsMatch(password))
            {
                valid = false;
                throw new Exception(errorMessage);
            }
            return valid;
        }

        public RefreshToken GenerateRefreshToken(User user)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpirationDate = DateTime.Now.AddDays(7),
                CreationDate = DateTime.Now,
                User = user,
                UserEmail = user.Email
            };

            return refreshToken;
        }

        public void SetRefreshToken(RefreshToken refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.ExpirationDate
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken",
                refreshToken.Token, cookieOptions);
        }
    }
}

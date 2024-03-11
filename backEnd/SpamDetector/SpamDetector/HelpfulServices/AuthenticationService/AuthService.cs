using Microsoft.IdentityModel.Tokens;
using SpamDetector.Features.UserManagement.Register.Dtos;
using SpamDetector.Models.UserManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace SpamDetector.HelpfulServices.AuthenticationService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #region Validation Regexes
        private static readonly Regex hasNumber = new Regex(@"[0-9]+");
        private static readonly Regex hasUpperChar = new Regex(@"[A-Z]+");
        private static readonly Regex hasMiniMaxChars = new Regex(@".{8,100}");
        private static readonly Regex hasLowerChar = new Regex(@"[a-z]+");
        private static readonly Regex hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
        private static readonly Regex checkEmail = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
        #endregion
        public AuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        #region Password Hashing
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
        #endregion

        #region Validate Password
        public bool ValidatePassword(string password)
        {
            var valid = true;

            ValidateCondition(password, null, "Password should not be empty.", valid);
            ValidateCondition(password, hasLowerChar, "Password should contain at least one lower case letter.", valid);
            ValidateCondition(password, hasUpperChar, "Password should contain at least one upper case letter.", valid);
            ValidateCondition(password, hasMiniMaxChars, "Password should not be lesser than 8 characters.", valid);
            ValidateCondition(password, hasNumber, "Password should contain at least one numeric value.", valid);
            ValidateCondition(password, hasSymbols, "Password should contain at least one special case character.", valid);

            return valid;
        }

        private static bool ValidateCondition(string input, Regex regex, string errorMessage, bool valid)
        {
            if (regex is not null)
            {
                if (!regex.IsMatch(input))
                {
                    valid = false;
                    throw new Exception(errorMessage);
                }
            }
            else if (string.IsNullOrWhiteSpace(input))
            {
                valid = false;
                throw new Exception(errorMessage);
            }
            return valid;
        }
        #endregion

        #region Validate Email
        public bool ValidateEmail(string email)
        {
            var valid = true;

            ValidateCondition(email, null, "Email should not be empty.", valid);
            ValidateCondition(email, checkEmail, "This is not a valid form for an email.", valid);

            return valid;
        }

        #endregion

        #region Refresh Token
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
        #endregion

        #region Password Reset Token
        public PasswordResetToken GetPasswordResetToken(User user)
        {
            var passwordResetToken = new PasswordResetToken
            {
                Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(16)),
                ExpirationDate = DateTime.Now.AddMinutes(5),
                User = user,
                UserEmail = user.Email
            };

            return passwordResetToken;
        }
        #endregion

    }
}

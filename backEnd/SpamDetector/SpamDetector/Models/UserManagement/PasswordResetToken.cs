namespace SpamDetector.Models.UserManagement
{
    public class PasswordResetToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string UserEmail { get; set;}
        public User User { get; set;}   
    }
}

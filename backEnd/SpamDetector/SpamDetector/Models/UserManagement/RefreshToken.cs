namespace SpamDetector.Models.UserManagement
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime ExpirationDate { get; set; } 
        public string UserEmail { get; set; }
        public User User { get; set; }
    }
}

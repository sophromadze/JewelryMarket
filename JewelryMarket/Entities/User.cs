using JewelryMarket.Models;

namespace JewelryMarket.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public decimal Balance { get; set; } = 0;
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string PersonalId { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; } = 0;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? TokenCreatedAt { get; set; }
        public DateTime? TokenUpdatedAt { get; set; }
    }
}

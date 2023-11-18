using System.Security.Cryptography;

namespace AuctionAPI.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public decimal Balance { get; set; } 
        public bool IsSeller { get; set; } 
        public bool IsAdmin { get; set; } 

        // Navigation properties
        public ICollection<Product> Products { get; set; } 
        public ICollection<Bid> Bids { get; set; } 
        public ICollection<Auction> WonAuctions { get; set; } 
        public ICollection<Organization> Organizations { get; set; } 
    }
}

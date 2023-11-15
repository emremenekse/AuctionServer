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
        public decimal Balance { get; set; } // Kullanıcının hesap bakiyesi
        public bool IsSeller { get; set; } // Kullanıcı bir satıcı mı?
        public bool IsAdmin { get; set; } // Kullanıcı bir satıcı mı?

        // Navigation properties
        public ICollection<Product> Products { get; set; } // Kullanıcının yüklediği ürünler
        public ICollection<Bid> Bids { get; set; } // Kullanıcının teklif verdiği açık artırmalar
        public ICollection<Auction> WonAuctions { get; set; } // Kullanıcının kazandığı açık artırmalar
        public ICollection<Organization> Organizations { get; set; } // Kullanıcının yönettiği organizasyonlar (eğer admin ise)
    }
}

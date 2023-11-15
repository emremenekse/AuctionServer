namespace AuctionAPI.Entities
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public int AdminUserId { get; set; } // Organizasyonu yaratan admin kullanıcının ID'si

        public bool IsActive { get; set; }
        // Navigation properties
        public User AdminUser { get; set; } // Organizasyonu yaratan admin kullanıcı
        public ICollection<Auction> Auctions { get; set; } // Organizasyon içindeki açık artırmalar
    }
}

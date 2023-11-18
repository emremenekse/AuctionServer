namespace AuctionAPI.Entities
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public int AdminUserId { get; set; }

        public bool IsActive { get; set; }
        
        public User AdminUser { get; set; } 
        public ICollection<Auction> Auctions { get; set; } 
    }
}

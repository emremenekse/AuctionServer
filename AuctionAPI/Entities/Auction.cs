using System.Text.Json.Serialization;

namespace AuctionAPI.Entities
{
    public class Auction
    {
        public int AuctionId { get; set; }
        public int ProductId { get; set; }
        public int OrganizationId { get; set; }
        public string AuctionName { get; set; }
        public decimal LastPrice { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsCompleted { get; set; }
        public int? WinnerUserId { get; set; }
        public string Description { get; set; }

        // Navigation properties
        public Product Product { get; set; } 
        public ICollection<Bid> Bids { get; set; } 
        public User Winner { get; set; } 
        public Organization Organization { get; set; }
    }
}

namespace AuctionAPI.Entities
{
    public class Bid
    {
        public int BidId { get; set; }
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public decimal Amount { get; set; } 
        public DateTime Timestamp { get; set; } 

        
        public User User { get; set; } 
        public Auction Auction { get; set; } 
    }
}

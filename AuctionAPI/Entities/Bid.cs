namespace AuctionAPI.Entities
{
    public class Bid
    {
        public int BidId { get; set; }
        public int UserId { get; set; }
        public int AuctionId { get; set; }
        public decimal Amount { get; set; } // Teklif miktarı
        public DateTime Timestamp { get; set; } // Teklif zaman damgası

        // Navigation properties
        public User User { get; set; } // Teklifi veren kullanıcı
        public Auction Auction { get; set; } // Teklifin yapıldığı açık artırma
    }
}

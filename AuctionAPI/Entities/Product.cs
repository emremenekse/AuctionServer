namespace AuctionAPI.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int ProviderId { get; set; } 
        public string? Image { get; set; } 

        
        public User Provider { get; set; } 
        public ICollection<Auction> Auctions { get; set; } 
    }
}

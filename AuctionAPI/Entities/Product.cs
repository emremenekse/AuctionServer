namespace AuctionAPI.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int ProviderId { get; set; } // Ürünü yükleyen kullanıcının ID'si
        public string? Image { get; set; } // Ürün resmi için eklenen alan

        // Navigation properties
        public User Provider { get; set; } // Ürünü yükleyen kullanıcı
        public ICollection<Auction> Auctions { get; set; } // Ürün için yapılan açık artırmalar
    }
}

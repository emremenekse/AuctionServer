namespace AuctionAPI.DTO
{
    public class ProductOperationDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int ProviderId { get; set; } // Ürünü yükleyen kullanıcının ID'si
        public string? Image { get; set; } // Ürün resmi için eklenen alan
    }
}

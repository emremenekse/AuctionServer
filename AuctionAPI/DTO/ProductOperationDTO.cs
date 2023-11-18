namespace AuctionAPI.DTO
{
    public class ProductOperationDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int ProviderId { get; set; } 
        public string? Image { get; set; } 
    }
}

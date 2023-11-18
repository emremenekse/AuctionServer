namespace AuctionAPI.DTO
{
    public class AuctionOperationDTO
    {
        public int AuctionId { get; set; }
        public int ProductId { get; set; }
        public int ProviderId { get; set; }
        public string? AuctionName { get; set; }
        public string? ProductName { get; set; }
        public string? ProviderUserName { get; set; }
        public decimal LastPrice { get; set; }
        public decimal? StartPrice { get; set; }
        public bool IsCompleted { get; set; }
        public int? AuctionWinnerId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? PhotoBytes { get; set; } 
        public string? AuctionDescription { get; set; } 
    }
}

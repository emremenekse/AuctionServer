using AuctionAPI.Data;
using AuctionAPI.Entities;
using AuctionAPI.Repository;

namespace AuctionAPI.Services
{
    public class BidService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        protected readonly AuctionDbContext _context;
        public BidService(IUserRepository userRepository, IBidRepository bidRepository, AuctionDbContext context, IProductRepository productRepository, IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
            _context = context;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _bidRepository = bidRepository;
        }
        public async Task<Bid> GetBidWithAuctionId(int auctionId)
        {
            var bid = await _bidRepository.GetByIdAsync(auctionId);
            return bid;
        }

        
        public async Task CreateNewBid(int userId, int auctionId, int amount)
        {
            var activeLastBid = await _bidRepository.GetByIdAsync(auctionId);
            if(activeLastBid == null)
            {
                var auctionInfo = await _auctionRepository.GetByIdAsync(auctionId);
                if(auctionInfo.LastPrice > amount)
                {
                    throw new Exception("Invalid Bid Price");
                }
            }
            else
            {
                if(amount < activeLastBid.Amount)
                {
                    throw new Exception("Invalid Bid Price");
                }
            }
            var newBid = new Bid()
            {
                UserId = userId,
                AuctionId = auctionId,
                Amount = amount,
                Timestamp = DateTime.UtcNow
            };
            await _bidRepository.AddAsync(newBid);
            await _bidRepository.SaveAsync();
        }
    }
}

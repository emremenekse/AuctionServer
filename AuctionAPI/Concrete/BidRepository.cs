using AuctionAPI.Data;
using AuctionAPI.Entities;
using AuctionAPI.Repository;

namespace AuctionAPI.Concrete
{
    public class BidRepository : Repository<Bid>, IBidRepository
    {
        public BidRepository(AuctionDbContext context) : base(context)
        {

        }
    
    }
}

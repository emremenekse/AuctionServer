using AuctionAPI.Data;
using AuctionAPI.Entities;
using AuctionAPI.Repository;

namespace AuctionAPI.Concrete
{
    public class AuctionRepository : Repository<Auction>, IAuctionRepository
    {
        public AuctionRepository(AuctionDbContext context): base(context)
        {
                
        }
    }
}

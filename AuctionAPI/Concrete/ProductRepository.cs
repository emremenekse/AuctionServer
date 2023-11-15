using AuctionAPI.Data;
using AuctionAPI.Entities;
using AuctionAPI.Repository;

namespace AuctionAPI.Concrete
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AuctionDbContext context) : base(context)
        {

        }
    }
}

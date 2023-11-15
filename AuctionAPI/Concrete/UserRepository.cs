using AuctionAPI.Data;
using AuctionAPI.Entities;
using AuctionAPI.Repository;

namespace AuctionAPI.Concrete
{
    public class UserRepository : Repository<User>, IUserRepository
    {


        public UserRepository(AuctionDbContext context) : base(context)
        {

        }



    }
}

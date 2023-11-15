using AuctionAPI.Data;
using AuctionAPI.Entities;
using AuctionAPI.Repository;

namespace AuctionAPI.Concrete
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(AuctionDbContext context) : base(context)
        {

        }
    }
}

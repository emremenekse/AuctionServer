using AuctionAPI.Concrete;
using AuctionAPI.Data;
using AuctionAPI.DTO;
using AuctionAPI.Entities;
using AuctionAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Services
{
    public class OrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;
        protected readonly AuctionDbContext _context;
        public OrganizationService(AuctionDbContext context,IOrganizationRepository organizationRepository, IUserRepository userRepository)
        {
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
            _context = context;
        }

        public async Task CreateOrganization(OrganizationOperationDTO organizationDTO)
        {
            var newUserList = await _userRepository.GetAllAsync();
            
            var adminInfo = newUserList.Where(x => x.Username == organizationDTO.AdminUserName).FirstOrDefault();
            if (adminInfo == null)
            {
                throw new Exception("Check your Admin UserName");
            }
            var organization = new Organization()
            {
                Name = organizationDTO.Name,
                AdminUserId = adminInfo.UserId,
            };
            await _organizationRepository.AddAsync(organization);
            await _organizationRepository.SaveAsync();
            await _context.Database.ExecuteSqlRawAsync("EXEC UpdateOrganizationStatus");
        }

        public async Task<List<Organization>> GetAllOrganizations()
        {
            var organizations = await _organizationRepository.GetAllAsync();
            
            var orderedOrganization = organizations.OrderByDescending(x => x.OrganizationId).ToList();
            return orderedOrganization;
        }
    }
}

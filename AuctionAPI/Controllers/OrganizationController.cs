using AuctionAPI.DTO;
using AuctionAPI.Entities;
using AuctionAPI.Repository;
using AuctionAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly OrganizationService _organizationService;
        


        public OrganizationController(OrganizationService organizationService)
        {
            _organizationService = organizationService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrganization ([FromBody] OrganizationOperationDTO organization)
        {
            await _organizationService.CreateOrganization(organization);
            return Ok(organization);
        }
        [HttpGet]
        public async Task<List<Organization>> GetOrganizations()
        {
            var organizations = await _organizationService.GetAllOrganizations();
            return (organizations);
        }
    }
}

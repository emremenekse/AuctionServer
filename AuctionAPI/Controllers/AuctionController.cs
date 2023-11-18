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
    public class AuctionController : ControllerBase
    {
        private readonly AuctionService _auctionService;
        private readonly IOrganizationRepository _organizationRepository;


        public AuctionController(IOrganizationRepository organizationRepository, AuctionService auctionService)
        {
            _auctionService = auctionService;
            _organizationRepository = organizationRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuction([FromBody] AuctionOperationDTO auction)
        {
            await _auctionService.CreateAuction(auction);
            return Ok(auction);
        }

        [HttpPost]
        public async Task<IActionResult> MoneyTransfer([FromBody] AuctionOperationDTO auction)
        {
            await _auctionService.MoneyTransfer(auction);
            return Ok(auction);
        }

        [HttpGet]
        public async Task<List<Auction>> GetAllAuctions()
        {
            var auctions = await _auctionService.GetAllAuctions();
            return auctions.ToList();
        }
        [HttpGet]
        public async Task<AuctionOperationDTO> GetAllAuctionsWithInclude([FromQuery] string? id)
        {
            var auctions = await _auctionService.GetAllAuctionsWithInclude(id);
            return auctions;
        }

        [HttpGet]
        public async Task<List<Auction>> GetAuctionsById([FromQuery] int id)
        {
            var userAuctionInfo = await _auctionService.GetByIdAuction(id);
            return userAuctionInfo.ToList();
        }

        [HttpGet]
        public async Task<List<AuctionOperationDTO>> GetAuctionsByName([FromQuery] string? id)
        {
            
            var userAuctionInfo = await _auctionService.GetByOrganizaionNameAuctions(id);
            return userAuctionInfo.ToList();
        }



    }
}

using AuctionAPI.Concrete;
using AuctionAPI.Data;
using AuctionAPI.DTO;
using AuctionAPI.Entities;
using AuctionAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System;

namespace AuctionAPI.Services
{
    public class AuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        protected readonly AuctionDbContext _context;
        public AuctionService(IUserRepository userRepository,AuctionDbContext context, IProductRepository productRepository,IOrganizationRepository organizationRepository,IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
            _organizationRepository = organizationRepository;
            _context = context;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task CreateAuction(AuctionOperationDTO auction)
        {
            
            var productInfo = new Product();
            var userList = await _userRepository.GetAllAsync();
            var productList = await _productRepository.GetAllAsync();
            var providerIdFind  = userList.Where(x => x.Username == auction.ProviderUserName).FirstOrDefault();
            var productExist = productList.Where(x => x.Name == auction.ProductName).FirstOrDefault();
            //Product Added
            if(productExist != null) {
                throw new Exception("Product Has AlreadyExist");
            }
            if(providerIdFind == null)
            {
                throw new Exception("Invalid Provider UserName");
            }
            productInfo.ProviderId = providerIdFind.UserId;
            productInfo.Name = auction.ProductName;
            await _productRepository.AddAsync(productInfo);
            await _productRepository.SaveAsync();
            //var sqlCommand = $"EXEC AddProduct @Name = {productInfo.Name}, @ProviderId = {productInfo.ProviderId}";

            //await _context.Database.ExecuteSqlRawAsync(sqlCommand);
            //OrganizationFind
            var organizations = await _organizationRepository.GetAllAsync();
            var currentOrganization = organizations.OrderByDescending(x => x.OrganizationId).FirstOrDefault();
            //Create Auction
            var newProductList = await _productRepository.GetAllAsync();
            var addedProduct = newProductList.Where(x => x.Name ==  auction.ProductName).FirstOrDefault();
            var auctionInfo = new Auction()
            {
                StartTime = DateTime.Now,
                EndTime= DateTime.Now.AddMinutes(5),
                LastPrice = auction.LastPrice,
                AuctionName = auction.AuctionName,
                ProductId = addedProduct.ProductId,
                IsCompleted = false,
                WinnerUserId = providerIdFind.UserId,
                OrganizationId = currentOrganization.OrganizationId
            };
            await _auctionRepository.AddAsync(auctionInfo);
            await _auctionRepository.SaveAsync();
        }

        public async Task<List<Auction>> GetAllAuctions()
        {
            var auctionList = await _auctionRepository.GetAllAsync();
            return auctionList.ToList();
        }
        public async Task<AuctionOperationDTO> GetAllAuctionsWithInclude(string? auctionName)
        {
            
            IEnumerable<Auction>? auctionList  = await _auctionRepository.GetAllWithIncludesAsync(q => q.Include(a => a.Product));
            var currentAuction = auctionList.Where(x => x.AuctionName == (auctionName)).FirstOrDefault();
            var auctionDTO = new AuctionOperationDTO()
            {
                AuctionId = currentAuction.AuctionId,
                ProductId = currentAuction.ProductId,
                AuctionName = currentAuction.AuctionName,
                ProductName = currentAuction.Product.Name,
                ProviderId = currentAuction.Product.ProviderId,
                LastPrice = currentAuction.LastPrice,
                IsCompleted = currentAuction.IsCompleted,
                StartTime = currentAuction.StartTime,
                EndTime = currentAuction.EndTime,
                Image = currentAuction.Product.Image,
            };

            return auctionDTO;
        }
        public async Task<List<Auction>> GetByIdAuction(int? id)
        {
            var auctionList = await _auctionRepository.GetAllAsync();
            var userAuctionList = auctionList.Where(x => x.WinnerUserId == id);
            return userAuctionList.ToList();
        }

        public async Task<List<AuctionOperationDTO>> GetByOrganizaionNameAuctions(string? organizationName)
        {
            List<AuctionOperationDTO>? auctionList = new List<AuctionOperationDTO>();
            if(organizationName != null)
            {
                var organizationWithAuctions = await _context.Organizations
    .Include(org => org.Auctions)
    .FirstOrDefaultAsync(org => org.Name == organizationName);

                if (organizationWithAuctions != null)
                {
                    foreach (var auction in organizationWithAuctions.Auctions)
                    {
                        AuctionOperationDTO dto = new AuctionOperationDTO
                        {
                            AuctionId = auction.AuctionId,
                            ProductId = auction.ProductId,
                            AuctionName = auction.AuctionName,
                            LastPrice = auction.LastPrice,
                            IsCompleted = auction.IsCompleted,
                            AuctionWinnerId = auction.WinnerUserId,
                            EndTime = auction.EndTime,
                            StartTime = auction.StartTime,
                        };
                        auctionList.Add(dto);
                    }
                }
            }
            else
            {

            }

            return auctionList;
        }
    }
}

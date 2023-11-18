using AuctionAPI.Concrete;
using AuctionAPI.Data;
using AuctionAPI.DTO;
using AuctionAPI.Entities;
using AuctionAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace AuctionAPI.Services
{
    public class AuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IProductRepository _productRepository;
        private readonly IBidRepository _bidRepository;
        private readonly IUserRepository _userRepository;
        protected readonly AuctionDbContext _context;
        public AuctionService(IBidRepository bidRepository,IUserRepository userRepository,AuctionDbContext context, IProductRepository productRepository,IOrganizationRepository organizationRepository,IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
            _organizationRepository = organizationRepository;
            _context = context;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _bidRepository = bidRepository;
        }
        public async Task MoneyTransfer(AuctionOperationDTO currentauction)
        {
            
           var auction = await  _auctionRepository.GetByIdAsync(currentauction.AuctionId);
            var product = await _productRepository.GetByIdAsync(auction.ProductId);
            var bidList = await _bidRepository.GetAllAsync();
            var currentBid = bidList.Where(x => x.AuctionId == currentauction.AuctionId).OrderByDescending(x => x.BidId).FirstOrDefault();
            await MoneyTransfer(product.ProviderId, currentBid.UserId, currentBid.Amount);
            await _auctionRepository.UpdateFieldAsync(auction.AuctionId, u => u.IsCompleted, true);
            await _auctionRepository.UpdateFieldAsync(auction.AuctionId, x => x.WinnerUserId, currentBid.UserId);
            await _userRepository.SaveAsync();

        }
        private async Task MoneyTransfer(int providerId, int buyerId, decimal amount)
        {
            var provider = await _userRepository.GetByIdAsync(providerId);
            var buyer = await _userRepository.GetByIdAsync(buyerId);
            if (buyer.Balance > amount)
            {
                provider.Balance += amount;
                await _userRepository.UpdateFieldAsync(provider.UserId, u => u.Balance, provider.Balance);
                
                buyer.Balance -= amount;
                await _userRepository.UpdateFieldAsync(buyer.UserId, u => u.Balance, buyer.Balance);
                await _userRepository.SaveAsync();
            }
            else
            {
                throw new Exception("Buyer Balance is not Enough!");
            }

        }
        public async Task CreateAuction(AuctionOperationDTO auction)
        {
            string base64Data = auction.PhotoBytes;
            int indexOfComma = base64Data.IndexOf(',');
            if (indexOfComma != -1)
            {
                base64Data = base64Data.Substring(indexOfComma + 1);
            }
            string newPhotoBytes = base64Data;
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
            productInfo.Image = newPhotoBytes;
            await _productRepository.AddAsync(productInfo);
            await _productRepository.SaveAsync();
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
                OrganizationId = currentOrganization.OrganizationId,
                Description = auction.AuctionDescription,
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
                PhotoBytes = (currentAuction.Product.Image),
                AuctionDescription = currentAuction.Description,
            };
            var bidList = await _bidRepository.GetAllAsync();
            var lastBid = bidList.Where( x=>x.AuctionId == auctionDTO.AuctionId).OrderByDescending(x => x.BidId).FirstOrDefault();
            if(lastBid != null)
            {
                auctionDTO.StartPrice = lastBid.Amount;
            }
            
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

                        var productList = await _productRepository.GetAllAsync();
                        var productInfo = productList.Where(x => x.ProductId == auction.ProductId).FirstOrDefault();
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
                            PhotoBytes =  (productInfo.Image),
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

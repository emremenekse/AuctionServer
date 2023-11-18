using AuctionAPI.Concrete;
using AuctionAPI.Entities;
using AuctionAPI.Repository;
using Microsoft.AspNetCore.SignalR;

namespace AuctionAPI.Hubs
{
    public class BidHub: Hub
    {
        private readonly IUserRepository _userRepository;
        private readonly IBidRepository _bidRepository;
        private readonly IAuctionRepository _auctionRepository;
        private readonly IProductRepository _productRepository;
        public BidHub(IProductRepository productRepository, IAuctionRepository auctionRepository, IBidRepository bidRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _bidRepository = bidRepository;
            _auctionRepository = auctionRepository;
            _productRepository = productRepository;
        }
        public async Task SendMessageAsync(string userName, int bid, int auctionId)
        {
            try
            {
                var userList = await _userRepository.GetAllAsync();
                var currentUser = userList.FirstOrDefault(x => x.Username == userName);
                
                var bidCreatedSuccessfully = false;
                if (currentUser == null)
                {
                    throw new Exception("Invalid UserName!");
                }
                if(currentUser.IsSeller || currentUser.IsAdmin)
                {
                    throw new Exception("Company member can not join an auction");
                }

                try
                {
                    var check = await CreateNewBid(currentUser.UserId, auctionId, bid);
                    bidCreatedSuccessfully = check;
                }
                catch (Exception ex)
                {
                    await Clients.Caller.SendAsync("ReceiveError", ex.Message);
                }

                var auctionInfo = await _auctionRepository.GetByIdAsync(auctionId);

                if (bidCreatedSuccessfully)
                {
                    await Clients.All.SendAsync("ReceiveBid", bid);
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveError", ex.Message);
            }
        }

        
        private async Task<bool> CreateNewBid(int userId, int auctionId, int amount)
        {
            try
            {
                var bidList = await _bidRepository.GetAllAsync();
                var activeLastBid = bidList.Where(x => x.AuctionId == auctionId).OrderByDescending(x => x.BidId).FirstOrDefault();
                var auctionList = await _auctionRepository.GetAllAsync();

                var auctionInfo = auctionList.Where(x => x.AuctionId == auctionId).FirstOrDefault();
                var productList = await _productRepository.GetAllAsync();
                var providerId = productList.Where(x => x.ProductId == auctionInfo.ProductId).FirstOrDefault().ProviderId;
                if(providerId == userId)
                {
                    throw new Exception("Can not Bid Your Own Product");
                }
                if (activeLastBid == null)
                {

                    if (auctionInfo.LastPrice > amount || auctionInfo.LastPrice == amount)
                    {
                        throw new Exception("Invalid Bid Price");
                    }
                }
                else
                {
                    if (amount < activeLastBid.Amount)
                    {
                        throw new Exception("Invalid Bid Price");
                    }
                }

                var currentUser = await _userRepository.GetByIdAsync(userId);
                if (amount > currentUser.Balance)
                {
                    throw new Exception("Not Enough Balance");
                }
                var newBid = new Bid()
                {
                    UserId = userId,
                    AuctionId = auctionId,
                    Amount = amount,
                    Timestamp = DateTime.UtcNow
                };
                if (newBid.Timestamp > auctionInfo.EndTime)
                {
                    throw new Exception("Auction Time is Over. Money Has Been Transferred. ");
                }
                await _bidRepository.AddAsync(newBid);
                await _bidRepository.SaveAsync();

                

                return true;
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveError", ex.Message);
                return false;
            }

        }
    }
}

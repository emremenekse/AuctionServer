using AuctionAPI.Concrete;
using AuctionAPI.Entities;
using AuctionAPI.Repository;

namespace AuctionAPI.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task CreateProduct(Product product)
        {
            await _productRepository.AddAsync(product);
            await _productRepository.SaveAsync();

        }

        public async Task<List<Product>> GetAllProducts()
        {
            var productList = await _productRepository.GetAllAsync();
            return productList.ToList();

        }
    }
}

using AuctionAPI.Entities;
using AuctionAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<List<Product>> GetAllProductList()
        {
            var users = await _productService.GetAllProducts();
            return users;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] Product product)
        {
            await _productService.CreateProduct(product);
            return Ok(product);
        }
    }
}

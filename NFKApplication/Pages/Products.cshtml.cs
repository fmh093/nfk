using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NFKApplication.Database;
using NFKApplication.Models;

namespace NFKApplication.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsModel> _logger;

        public List<Product> Products { get; set; } = new List<Product>();

        public ProductsModel(IProductRepository productRepository, ILogger<ProductsModel> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public void OnGet()
        {
            var mappedProducts = new List<Product>();
            var dbProducts = _productRepository.GetAll();

            foreach (var product in dbProducts)
            {
                mappedProducts.Add(new Product()
                {
                    Name = product.Name,
                    ImagePath = "images/" + product.ImagePath,
                    Price = product.Price,
                    Sku = product.Sku
                });
            }

            Products = mappedProducts;
        }
    }
}
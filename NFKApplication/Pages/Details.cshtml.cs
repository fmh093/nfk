using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NFKApplication.Database;
using NFKApplication.Models;
using NFKApplication.Services;

namespace NFKApplication.Pages
{
    public class DetailsModel : PageModel
    {
        public Product Product { get; set; } = new Product();

        private readonly IProductRepository _productRepository;
        private readonly IBasketService _basketService;

        public DetailsModel(IProductRepository productRepository, IBasketService basketService)
        {
            _productRepository = productRepository;
            _basketService = basketService;
        }

        public IActionResult OnGet(string sku)
        {
            _ = _basketService.GetBasket(HttpContext); // ensure basket

            var dbProduct = _productRepository.Get(sku);

            if (Product == null)
            {
                return NotFound();
            }

            Product = new Product()
            {
                Name = dbProduct.Name,
                ImagePath = "images/" + dbProduct.ImagePath,
                Price = dbProduct.Price,
                Sku = dbProduct.Sku,
            };

            return Page();
        }
    }
}

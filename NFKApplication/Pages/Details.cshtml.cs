using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NFKApplication.Database;
using NFKApplication.Models;

namespace NFKApplication.Pages
{
    public class DetailsModel : PageModel
    {
        public Product Product { get; set; } = new Product();

        private readonly IProductRepository _productRepository;

        public DetailsModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult OnGet(string sku)
        {
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

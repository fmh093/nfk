using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NFKApplication.Database;
using NFKApplication.Models;
using NFKApplication.Services;

namespace NFKApplication.Pages
{
    public class BasketsModel : PageModel
    {
        public Basket Basket { get; set; } = new Basket();

        private readonly IBasketService _basketService;

        public BasketsModel(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public IActionResult OnGet()
        {
            Basket = _basketService.GetBasket(HttpContext);
            return Page();
        }
    }
}

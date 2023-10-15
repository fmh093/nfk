using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NFKApplication.Database;
using NFKApplication.Models;
using NFKApplication.Services;

namespace NFKApplication.Pages
{
    public class ReceiptModel : PageModel
    {
        public Basket Basket { get; set; } = new Basket();

        private readonly IBasketService _basketService;

        public ReceiptModel(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public IActionResult OnGet()
        {
            _basketService.CompleteCheckout(HttpContext);
            Basket = _basketService.GetBasket(HttpContext);
            return Page();
        }
    }
}

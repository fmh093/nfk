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

        public IActionResult OnGet(int basketId)
        {
            Basket = _basketService.GetBasket(basketId);
            _basketService.CompleteCheckout(HttpContext, basketId);
            return Page();
        }
    }
}

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

        private readonly IBasketRepository _basketRepository;
        private readonly IBasketService _basketService;

        public BasketsModel(IBasketRepository basketRepository, IBasketService basketService)
        {
            _basketRepository = basketRepository;
            _basketService = basketService;
        }

        public IActionResult OnGet()
        {
            Basket = _basketService.GetBasket(HttpContext);
            return Page();
        }

        int GenerateNewBasketId()
        {
            // todo 
            return new Random().Next(1000, 10000);
        }
    }
}

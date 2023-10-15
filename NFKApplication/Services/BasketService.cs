using NFKApplication.Database;
using NFKApplication.Models;

namespace NFKApplication.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        public BasketService(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        private int GetOrSetBasketId(HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey("BasketId"))
            {
                if (int.TryParse(context.Request.Cookies["BasketId"], out var id))
                {
                    return id;
                }
            }

            var basketId = _basketRepository.GetNewBasketId();
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddYears(1)
            };
            context.Response.Cookies.Append("BasketId", basketId.ToString(), cookieOptions);

            return basketId;
        }

        public Basket GetBasket(HttpContext context)
        {
            var basketId = GetOrSetBasketId(context);
            return _basketRepository.GetOrCreateBasket(basketId);
        }

        public void CompleteCheckout(HttpContext context)
        {
            var basketId = GetOrSetBasketId(context);
            _basketRepository.CompleteBasket(basketId);
        }
    }

    public interface IBasketService
    {
        Basket GetBasket(HttpContext context);
        void CompleteCheckout(HttpContext context);
    }
}

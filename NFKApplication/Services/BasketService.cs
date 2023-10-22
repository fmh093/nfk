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
            return GetBasket(basketId);
        }

        public Basket GetBasket(int basketId)
        {
            return _basketRepository.GetOrCreateBasket(basketId);
        }

        public void CompleteCheckout(HttpContext context, int basketId)
        {
            _basketRepository.CompleteBasket(basketId);
            ClearBasket(context);
        }

        private void ClearBasket(HttpContext context)
        {
            context.Response.Cookies.Delete("BasketId");
        }
    }

    public interface IBasketService
    {
        Basket GetBasket(HttpContext context);
        Basket GetBasket(int basketId);
        void CompleteCheckout(HttpContext context, int basketId);
    }
}

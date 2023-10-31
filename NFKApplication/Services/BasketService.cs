using NFKApplication.Database;
using NFKApplication.Models;

namespace NFKApplication.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ILogger<IBasketService> _logger;
        public BasketService(IBasketRepository basketRepository, ILogger<IBasketService> logger)
        {
            _basketRepository = basketRepository;
            _logger = logger;
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

        public Basket AddToBasket(int basketId, string sku, int amount)
        {
            return _basketRepository.AddToBasket(basketId, sku, amount);
        }

        public void UpdateBasket(Basket basket)
        {
            _basketRepository.UpdateBasket(basket);
        }

        public void CompleteCheckout(HttpContext context, int basketId)
        {
            _basketRepository.CompleteBasket(basketId);
            // todo send basket to erp system
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
        Basket AddToBasket(int basketId, string sku, int amount);
        void UpdateBasket(Basket basket);
        void CompleteCheckout(HttpContext context, int basketId);
    }
}

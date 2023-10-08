using Microsoft.EntityFrameworkCore;
using NFKApplication.Models;
using System.Diagnostics.CodeAnalysis;

namespace NFKApplication.Database
{
    public class BasketRepository : IBasketRepository
    {

        private readonly AppDbContext _context;

        public BasketRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool TryGetBasket(int id, [NotNullWhen(true)] out Basket? basket)
        {
            var dbBasket = _context.Baskets.FirstOrDefault(b => b.Id == id);
            if(dbBasket == null)
            {
                basket = null;
                return false;
            }

            basket = Basket.MapToBasket(dbBasket);
            return true;
        }

        public Basket AddToBasket(int basketId, string sku, int amount)
        {
            // todo changed sku to string, change sku to string everywhere, fix basket
            var basketFound = TryGetBasket(basketId, out var basket);
            if (!basketFound)
                throw new Exception("Basket not found");

            var lineItem = basket!.LineItems.FirstOrDefault(li => li.Sku == sku);

        }
    }

    public interface IBasketRepository
    {
        bool TryGetBasket(int id, out Basket? basket);
        Basket AddToBasket(string sku, int amount);
    }
}

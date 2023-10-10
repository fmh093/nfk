using Microsoft.EntityFrameworkCore;
using NFKApplication.Database.Models;
using NFKApplication.Models;
using System.Diagnostics.CodeAnalysis;

namespace NFKApplication.Database
{
    public class BasketRepository : IBasketRepository
    {

        private readonly AppDbContext _context;
        private readonly object _dbLock = new object();

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

        public Basket GetOrCreateBasket(int id)
        {
            var basketFound = TryGetBasket(id, out var basket);
            if (!basketFound)
            {
                basket = new Basket() { Id = id };
                SaveBasket(basket);
            }

            return basket!;
        }

        public Basket AddToBasket(int basketId, string sku, int amount)
        {
            var basketDto = _context.Baskets.SingleOrDefault(b => b.Id == basketId);
            if (basketDto == null)
                throw new Exception("Basket not found");

            var basket = Basket.MapToBasket(basketDto);

            var lineItem = basket.LineItems.FirstOrDefault(li => li.Sku == sku);
            if (lineItem == null)
            {
                lineItem = new LineItem { Sku = sku, Amount = amount };
                basket.LineItems.Add(lineItem);
            }
            else
            {
                lineItem.Amount += amount;
            }

            basketDto = BasketDto.MapToBasketDto(basket);

            _context.Entry(basketDto).State = EntityState.Modified;
            _context.SaveChanges();

            return basket;
        }

        public void SaveBasket(Basket basket)
        {
            var basketDto = BasketDto.MapToBasketDto(basket);
            var existingBasket = _context.Baskets.Find(basket.Id);

            if (existingBasket != null)
            {
                _context.Entry(existingBasket).CurrentValues.SetValues(basketDto);
            }
            else
            {
                _context.Baskets.Add(basketDto);
            }

            _context.SaveChanges();
        }

        public int GetNewBasketId()
        {
            int highestId = _context.Baskets.Max(b => (int?)b.Id) ?? 1000;
            return highestId += 1;
        }
    }

    public interface IBasketRepository
    {
        bool TryGetBasket(int id, [NotNullWhen(true)] out Basket? basket);
        public Basket GetOrCreateBasket(int id);
        Basket AddToBasket(int basketId, string sku, int amount);
        void SaveBasket(Basket basket);
        int GetNewBasketId();
    }
}

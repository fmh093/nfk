using Microsoft.EntityFrameworkCore;
using NFKApplication.Database.Models;
using NFKApplication.Models;
using System.Diagnostics.CodeAnalysis;

namespace NFKApplication.Database
{
    public class BasketRepository : IBasketRepository
    {

        private readonly AppDbContext _context;
        private readonly IProductRepository _productRepository;

        public BasketRepository(AppDbContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
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
            if (basketFound)
            {
                return basket!;
            }
            return CreateBasket(id);
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
                var product = _productRepository.Get(sku);
                lineItem = new LineItem { Name = product.Name, Price = product.Price, Sku = product.Sku, Amount = amount };
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

        public Basket CreateBasket(int id)
        {
            var basket = new Basket() { Id = id };
            var basketDto = BasketDto.MapToBasketDto(basket);
            _context.Baskets.Add(basketDto);
            _context.SaveChanges();
            return basket;
        }

        public void UpdateBasket(Basket basket)
        {
            var basketDto = _context.Baskets.SingleOrDefault(b => b.Id == basket.Id);
            if (basketDto == null)
                return; // throw?

            var mappedBasketDto = BasketDto.MapToBasketDto(basket);

            //var existingBasket = _context.Baskets.Find(basket.Id);

            _context.Entry(basketDto).CurrentValues.SetValues(mappedBasketDto);

            _context.Entry(basketDto).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void CompleteBasket(int basketId)
        {
            var basketDto = _context.Baskets.SingleOrDefault(b => b.Id == basketId);
            if (basketDto == null)
                throw new Exception("Basket not found");

            var basket = Basket.MapToBasket(basketDto);
            basket.IsCompleted = true;

            basketDto = BasketDto.MapToBasketDto(basket);
            _context.Entry(basketDto).State = EntityState.Modified;
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
        void UpdateBasket(Basket basket);
        void CompleteBasket(int basketId);
        int GetNewBasketId();
    }
}

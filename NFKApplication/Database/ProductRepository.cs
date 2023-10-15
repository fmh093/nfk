using Microsoft.EntityFrameworkCore;
using NFKApplication.Models;

namespace NFKApplication.Database
{
    public class ProductRepository : IProductRepository
    {

        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public Product Get(string sku)
        {
            return _context.Products.First(p => p.Sku == sku);
        }

        public List<Product> GetAll()
        {
            var products = _context.Products.ToList();
            return products;
        }
    }

    public interface IProductRepository
    {
        Product Get(string sku);
        List<Product> GetAll();
    }
}

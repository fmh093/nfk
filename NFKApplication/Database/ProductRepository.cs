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
            return _context.Products
                        .FromSqlRaw($"SELECT * FROM Products WHERE Sku = '{sku}'")
                        .First();
        }

        public List<Product> GetAll()
        {
            return _context.Products
                        .FromSqlRaw("SELECT * FROM Products")
                        .ToList();
        }
    }

    public interface IProductRepository
    {
        Product Get(string sku);
        List<Product> GetAll();
    }
}

using NFKApplication.Database.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace NFKApplication.Models
{
    public class Basket
    {
        [Key]
        public int Id { get; set; }
        public List<LineItem> LineItems { get; set; } = new List<LineItem>();

        public static Basket MapToBasket(BasketDto dbBasket)
        {
            return new Basket
            {
                Id = dbBasket.Id,
                LineItems = JsonSerializer.Deserialize<List<LineItem>>(dbBasket.LineItemsJson)
            };
        }
    }
}

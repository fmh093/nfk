using NFKApplication.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace NFKApplication.Database.Models
{
    public class BasketDto
    {
        [Key]
        public int Id { get; set; }
        public string? LineItemsJson { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public static BasketDto MapToBasketDto(Basket basket)
        {
            return new BasketDto
            {
                Id = basket.Id,
                LineItemsJson = JsonSerializer.Serialize(basket.LineItems)
            };
        }
    }
}

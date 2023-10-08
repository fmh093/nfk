using NFKApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace NFKApplication.Database.Models
{
    public class BasketDto
    {
        [Key]
        public int Id { get; set; }
        public string? LineItemsJson { get; set; }
    }
}

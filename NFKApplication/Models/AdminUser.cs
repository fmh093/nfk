using System.ComponentModel.DataAnnotations;

namespace NFKApplication.Models
{
    public class AdminUser
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

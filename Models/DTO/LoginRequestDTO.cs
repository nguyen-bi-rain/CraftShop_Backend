using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CraftShop.API.Models.DTO
{
    public class LoginRequestDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

using System.Security.Principal;

namespace CraftShop.API.Models.DTO
{
    public class RegisterationDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}

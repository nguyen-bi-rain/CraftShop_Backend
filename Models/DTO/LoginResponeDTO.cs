namespace CraftShop.API.Models.DTO
{
    public class LoginResponeDTO
    {
        public UserDTO User { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }

    }
}

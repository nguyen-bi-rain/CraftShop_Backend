using CraftShop.API.Models.DTO;

namespace CraftShop.API.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<LoginResponeDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterationDTO registerationDTO);
        bool IsUniqueUser(string username);
        string GetUserIdFromToken(string token);
    }
}

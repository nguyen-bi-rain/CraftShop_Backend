using CraftShop.API.Models.DTO;
using CraftShop.API.Models;

namespace CraftShop.API.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<LoginResponeDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterationDTO registerationDTO);
        bool IsUniqueUser(string username);
        string GetUserIdFromToken(string token);
        Task<UserDTO> GetUserForAccount(string token);
        Task<bool> ChangeUserPassword(string token, string CurrentPassword,string  newPassword);
        Task UpdateUser(UserDTO userDTO);
        Task DeleteUser(string userId);
        Task<List<UserDTO>> GetAllUsers();
        Task ChangeUserPhoto(string token,IFormFile image);
        void DeleteOldUserPhoto(string userId);
        
    }
}

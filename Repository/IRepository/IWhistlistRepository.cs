using CraftShop.API.Models;
using Microsoft.Build.Framework;

namespace CraftShop.API.Repository.IRepository
{
    public interface IWishlistRepository
    {
        Task<List<Wishlist>> GetWishlistsAsync();
        Task AddToWishListAsync(int productId);
        Task RemoveFromWishListAsync(int productId); 
        
    }
}
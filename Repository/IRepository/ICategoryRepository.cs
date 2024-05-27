using CraftShop.API.Models;

namespace CraftShop.API.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> UpdateCategoryAsync(Category category);
    }
}

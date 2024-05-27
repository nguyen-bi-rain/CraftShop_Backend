using CraftShop.API.Data;
using CraftShop.API.Models;
using CraftShop.API.Repository.IRepository;

namespace CraftShop.API.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            category.UpdatedDate = DateTime.Now;
            _db.Update(category);
            await _db.SaveChangesAsync();
            return category;
        }
    }
}

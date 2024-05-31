using CraftShop.API.Data;
using CraftShop.API.Models;
using CraftShop.API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CraftShop.API.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public async Task<List<Product>> SearchProductAsync(ProductSearch fllter)
        {
            IQueryable<Product> query = _db.Products.Include(x => x.Category);
            if(fllter.CategoryId != null){
                query = query.Where(x => x.CategoryId == fllter.CategoryId);
            }
            if(fllter.Name != null){
                query = query.Where(x => x.ProductName.ToLower().Contains(fllter.Name.ToLower()));
            }
            if(fllter.MinPrice != null){
                query = query.Where(x => x.ProductPrice >= fllter.MinPrice);
            }
            if(fllter.MaxPrice != null){
                query = query.Where(x => x.ProductPrice <= fllter.MaxPrice);
            
            }
            return await query.ToListAsync();
        }

        public async Task<Product> UpdateProductAsync(Product entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}

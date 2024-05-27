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

       
        public async Task<Product> UpdateProductAsync(Product entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}

using CraftShop.API.Models;

namespace CraftShop.API.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> UpdateProductAsync(Product entity);
        //Task<Product> SearchProductAsync(ProductSearch product);
    }
}

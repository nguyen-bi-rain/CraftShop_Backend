using CraftShop.API.Models;
using CraftShop.API.Models.DTO;

namespace CraftShop.API.Repository.IRepository
{
    public interface IProductImageRepository
    {
        Task CreateImage(ProductImageCreatedDTO dto);
        Task<ProductImage> GetProductImage(int id);

    }
}

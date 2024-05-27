using System.ComponentModel.DataAnnotations.Schema;

namespace CraftShop.API.Models.DTO
{
    public class ProductImageCreatedDTO
    {
        public int ProductId { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}

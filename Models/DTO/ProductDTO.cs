using System.ComponentModel.DataAnnotations.Schema;

namespace CraftShop.API.Models.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductStock { get; set; }
        public string ProductSKU { get; set; }
        public double ProductPrice { get; set; }
        public ProductImageDTO ProductImage { get; set; }
        public CategoryDTO Category { get; set; }
        public int CategoryId { get; set; }
        public string ProductImageId { get; set; }
    }
}
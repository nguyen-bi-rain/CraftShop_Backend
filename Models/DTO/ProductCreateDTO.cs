using System.ComponentModel.DataAnnotations.Schema;

namespace CraftShop.API.Models.DTO
{
    public class ProductCreateDTO
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductStock { get; set; }
        public string ProductSKU { get; set; }
        public double ProductPrice { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

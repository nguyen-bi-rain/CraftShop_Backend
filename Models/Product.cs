using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftShop.API.Models
{
    public class Product

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id  { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductStock { get; set; }
        public string ProductSKU { get; set; }
        public double ProductPrice { get; set; }
        public Category Category { get; set; }
        public ProductImage ProductImage { get; set; }
        [ForeignKey(nameof(ProductImage))]
        public string ProductImageId  { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}

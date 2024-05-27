using System.ComponentModel.DataAnnotations.Schema;

namespace CraftShop.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        [NotMapped]
        public ICollection<Product> Products { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

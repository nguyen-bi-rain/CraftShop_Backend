using System.ComponentModel.DataAnnotations.Schema;

namespace CraftShop.API.Models
{
    public class ProductImage
    {
        public string  Id { get; set; }
        public int ProductId { get; set; }
        public string ImageThumb { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        [NotMapped]
        public List<IFormFile> Images { get; set; }
        [NotMapped]
        public Product Product { get; set; }

    }
}

using System.Security.Permissions;

namespace CraftShop.API.Models
{
    public class ProductSearch
    {   
        public int? CategoryId { get; set; }
        public string? Name { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
    }
}
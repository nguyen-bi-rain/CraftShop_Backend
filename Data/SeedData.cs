using CraftShop.API.Models.DTO;

namespace CraftShop.API.Data
{
    public static class SeedData
    {
        public static void Initial(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<CategoryCreatedDTO>()
                {
                    new CategoryCreatedDTO(categoryName :"Gom",createdDate : DateTime.Now),
                };
            }

        }
    }
}

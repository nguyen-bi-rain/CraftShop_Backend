namespace CraftShop.API.Models.DTO
{
    public class CategoryCreatedDTO
    {
        public CategoryCreatedDTO(string categoryName, DateTime createdDate)
        {
            CategoryName = categoryName;
            CreatedDate = createdDate;
        }

        public string CategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

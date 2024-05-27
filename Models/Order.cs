namespace CraftShop.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public Decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public string ApplicationUserId { get; set; }
        public int PaymentId { get; set; }  
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Payment Payment { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

    }
}

namespace CraftShop.API.Models.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public string ApplicationUserId { get; set; }
        public int PaymentId { get; set; }
    }
}

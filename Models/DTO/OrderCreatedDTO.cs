namespace CraftShop.API.Models.DTO;

public class OrderCreatedDTO{

        public DateTime OrderDate { get; set; }
        public Decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public string ApplicationUserId { get; set; }
        public int PaymentId { get; set; }  
        public DateTime CreatedDate { get; set; }
}
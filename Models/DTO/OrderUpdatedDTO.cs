namespace CraftShop.API.Models.DTO;

public class OrderUpdatedDTO{

        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public Decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public string ApplicationUserId { get; set; }
        public int PaymentId { get; set; }  
        public DateTime UpdatedDate { get; set; }
}
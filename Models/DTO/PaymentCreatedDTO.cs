namespace CraftShop.API.Models.DTO
{
    public class PaymentCreatedDTO{
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public Decimal Amount { get; set; }
        public string ApplicationUserId { get; set; }
    }
}

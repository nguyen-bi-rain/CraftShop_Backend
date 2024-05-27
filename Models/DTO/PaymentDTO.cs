namespace CraftShop.API.Models.DTO

{

    public class PaymentDTO
    {
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public Decimal Amount { get; set; }
        public string ApplicationUserId { get; set; }
        //user id   
    }
}
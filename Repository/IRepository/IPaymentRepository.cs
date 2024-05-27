using CraftShop.API.Models;
using System.Net;

namespace CraftShop.API.Repository.IRepository
{
    public interface IPaymentRepository
    {
        Task<List<Payment>> GetAllPayment();
        Task<Payment> GetPayment(int? id);
        Task CreatedPayment(Payment payment,string token);
        Task DeletePayment(Payment payment);
    }
}

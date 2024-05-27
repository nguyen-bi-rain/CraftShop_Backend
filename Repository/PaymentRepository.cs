using CraftShop.API.Data;
using CraftShop.API.Models;
using CraftShop.API.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CraftShop.API.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserRepository _userRepository;

        public PaymentRepository(ApplicationDbContext db, IUserRepository userRepository)
        {
            _db = db;
            _userRepository = userRepository;
        }

        public async Task CreatedPayment(Payment payment, string token)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    payment.PaymentDate = DateTime.Now;
                    payment.ApplicationUserId = _userRepository.GetUserIdFromToken(token);
                    _db.Add(payment);
                    await _db.SaveChangesAsync();
                }
                catch(System.Exception){
                    transaction.Rollback();
                    throw;
                }
            }

        }

        public async Task DeletePayment(Payment payment)
        {
                
            _db.Remove(payment);
            await _db.SaveChangesAsync();

        }

        public async Task<List<Payment>> GetAllPayment()
        {
            List<Payment> PaymentList = await _db.PayMent.ToListAsync();
            return PaymentList;
        }
        public Task<Payment> GetPayment(int? id)
        {
            var order = _db.PayMent.FirstOrDefaultAsync(x => x.Id == id);
            return order;
        }

        
    }
}

using CraftShop.API.Data;
using CraftShop.API.Models;
using CraftShop.API.Repository.IRepository;

namespace CraftShop.API.Repository
{

    public class OrderItemRepository : Repository<OrderItem>,IOrderItemRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderItemRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public  async Task<OrderItem> UpdateOrderItem(OrderItem order)
        {
            _db.Update(order);
            await _db.SaveChangesAsync();
            return order;
        }
    }
}

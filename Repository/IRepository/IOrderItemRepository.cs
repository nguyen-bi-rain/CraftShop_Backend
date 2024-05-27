using CraftShop.API.Models;

namespace CraftShop.API.Repository.IRepository
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<OrderItem> UpdateOrderItem(OrderItem order);
    }
}

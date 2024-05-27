using CraftShop.API.Models;
using CraftShop.API.Models.DTO;

namespace CraftShop.API.Repository.IRepository
{
    public interface IOrderRepository 
    {
        Task<List<Order>> GetOrdersAsync(string? status);
        Task<Order> GetOrderAsync(int? id);
        Task CreateOrderAsync(Order order,string token);
        Task<Order> UpdateOrderAsync(Order order);
        Task<Order> UpdateStatusOrderAsync(int? id,string status);
        Task DeleteOrder(Order order);

    }
}

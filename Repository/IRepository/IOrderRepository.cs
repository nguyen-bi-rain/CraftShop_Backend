using CraftShop.API.Models;
using CraftShop.API.Models.DTO;

namespace CraftShop.API.Repository.IRepository
{
    public interface IOrderRepository 
    {
        //retrive all orders 
        Task<List<Order>> GetOrdersAsync(string? status);
        //retrive order by id order
        Task<Order> GetOrderAsync(int? id);
        // get order by user id
        Task<Order> GetOrderByUser(string? token);
        // create new order
        Task CreateOrderAsync(Order order,string token);
        //update all field in order
        Task<Order> UpdateOrderAsync(Order order);
        //updae orderstatus by id (can use http patch)
        Task<Order> UpdateStatusOrderAsync(int? id,string status);
        //delete order 
        Task DeleteOrder(Order order);

    }
}

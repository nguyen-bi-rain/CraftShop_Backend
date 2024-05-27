using AutoMapper;
using CraftShop.API.Data;
using CraftShop.API.Models;
using CraftShop.API.Models.DTO;
using CraftShop.API.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CraftShop.API.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserRepository _userRepository;

        public OrderRepository(ApplicationDbContext db, IUserRepository userRepository)
        {
            _db = db;
            _userRepository = userRepository;
        }

        public Task CreateOrderAsync(Order order, string token)
        {
            var user = _userRepository.GetUserIdFromToken(token);
            order.ApplicationUserId = user;
            order.OrderDate = DateTime.Now;
            _db.Orders.Add(order);
            _db.SaveChangesAsync();
            return Task.CompletedTask;
        }

        public async Task<List<Order>> GetOrdersAsync(string status)
        {
            IQueryable<Order> OrderList = _db.Orders.Include(x => x.OrderItems);
            if(!string.IsNullOrEmpty(status))
            {
                OrderList = OrderList.Where(x => x.OrderStatus == status);
            }
            return await OrderList.ToListAsync();
        }

        public Task<Order> GetOrderAsync(int? id)
        {
            var order = _db.Orders.Include(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id == id);
            return order;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            order.UpdatedDate = DateTime.Now;
            _db.Update(order);
            _db.SaveChangesAsync();
            return order;   
        }

        public async Task<Order> UpdateStatusOrderAsync(int? id,string status)
        {
            var order = _db.Orders.FirstOrDefault(x => x.Id == id);
            order.OrderStatus = status;
            _db.Update(order);
            _db.SaveChangesAsync();
            return order;
        }

        public async Task DeleteOrder(Order order)
        {
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
        }
    }
}

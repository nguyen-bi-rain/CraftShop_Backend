﻿namespace CraftShop.API.Models.DTO
{
    public class OrderItemCreatedDTO
    {
        public int Quantity { get; set; }
        public Decimal Price { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
    }
}

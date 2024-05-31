using AutoMapper;
using CraftShop.API.Models;
using CraftShop.API.Models.DTO;


namespace CraftShop.API
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {

            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<Product, ProductUpdateDTO>().ReverseMap();

            CreateMap<Category, CategoryCreatedDTO>().ReverseMap();
            CreateMap<Category,CategoryDTO>().ReverseMap();
            
            CreateMap<ApplicationUser, UserDTO>().ReverseMap();

            CreateMap<ProductImage,ProductImageCreatedDTO>().ReverseMap();
            CreateMap<ProductImage, ProductImageDTO>().ReverseMap();

            CreateMap<Order,OrderDTO>().ReverseMap();
            CreateMap<Order,OrderCreatedDTO>().ReverseMap();
            CreateMap<Order, OrderUpdatedDTO>().ReverseMap();

            CreateMap<Payment,PaymentDTO>().ReverseMap();
            CreateMap<Payment, PaymentCreatedDTO>().ReverseMap();
            
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<OrderItemDTO,OrderItemCreatedDTO>().ReverseMap();
        }
    }
}

using System.Net;
using AutoMapper;
using CraftShop.API.Models;
using CraftShop.API.Models.DTO;
using CraftShop.API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        protected APIRespone _response;

        public OrderController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            this._response = new APIRespone();
        }
        [HttpGet]
        [Authorize("admin,customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIRespone>> GetOrders(string OrderStatus)
        {
            ICollection<Order> orders;
            try
            {
                orders = await _orderRepository.GetOrdersAsync(OrderStatus);
                if(orders == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { "No orders found" };
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<List<OrderDTO>>(orders);
                _response.Status = HttpStatusCode.OK;
                return Ok(_response);
                
            }catch(Exception e){
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.Message };
            }
            return _response;


        }
        [Authorize("admin,customer")]

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIRespone>> GetOrder(int? id){
            try{
                if(id == null){
                    _response.IsSuccess =false;
                    _response.Status = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                Order order = await _orderRepository.GetOrderAsync(id);
                if(order == null){
                    _response.IsSuccess = false;
                    _response.Status = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<OrderDTO>(order);
                _response.Status = HttpStatusCode.OK;
            }catch(Exception e){
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { e.Message };
            }
            return _response;
        }
        [Authorize("admin,customer")]

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIRespone>> CreateOrder([FromBody] OrderCreatedDTO dto,string token)
        {
            try
            {
                
                if (dto == null)
                {
                    return BadRequest(dto);
                }

                Order model = _mapper.Map<Order>(dto);

                await _orderRepository.CreateOrderAsync(model,token);
                _response.Result = _mapper.Map<OrderDTO>(model);
                _response.Status = HttpStatusCode.Created;
                return Ok(model);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }
        [Authorize("admin,customer")]

        [HttpPut("id:int", Name = "UpdateParital")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIRespone>> UpdateOrderStatus(int? id, string status)
        {
            try
            {
                if(id == null)
                {
                    return BadRequest();
                }
                var order = _orderRepository.GetOrderAsync(id);
                if(order == null)
                {
                    return NotFound();
                }
               await _orderRepository.UpdateStatusOrderAsync(id,status);
                _response.Status= HttpStatusCode.NoContent;
                return _response;
                
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }
        [Authorize("admin,customer")]
        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIRespone>> UpdateOrder(int id, [FromBody] OrderUpdatedDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest(updateDTO);
                }
                
                Order model = _mapper.Map<Order>(updateDTO);

               
                await _orderRepository.UpdateOrderAsync(model);
                _response.Status = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }
        [Authorize("admin,customer")]
        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIRespone>> DeleteOrder(int? id)
        {
            try
            {

                if (id == null)
                {
                    return BadRequest();
                }
                var order = await _orderRepository.GetOrderAsync(id);
                if(order == null)
                {
                    return NotFound();

                }
               await _orderRepository.DeleteOrder(order);
                _response.Status = HttpStatusCode.NoContent;
                return _response;

            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message};
            }
            return _response;
        }
    }
}

using AutoMapper;
using CraftShop.API.Models;
using CraftShop.API.Models.DTO;
using CraftShop.API.Repository;
using CraftShop.API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.VisualBasic;
using NuGet.Protocol;
using System.Net;
using System.Net.WebSockets;

namespace CraftShop.API.Controllers
{
    [Route("api/orderItem")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;
        protected APIRespone _response;

        public OrderItemController(IOrderItemRepository orderItemRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
            this._response = new APIRespone();
        }
        [Authorize(Roles = "customer,admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIRespone>> CreateOrderItem([FromBody] OrderItemCreatedDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest();
                }

                OrderItem model = _mapper.Map<OrderItem>(dto);
                await _orderItemRepository.CreateAsync(model);
                _response.Result = _mapper.Map<OrderItemDTO>(model);
                _response.Status = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.IsSuccess = false;
            }
            return _response;
        }
        [Authorize(Roles = "admin,customer")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIRespone>> GetOrderItemByOrderID(int? OrderId)
        {
            try
            {
                var order = await _orderItemRepository.GetAllNoPagination(u => u.OrderId == OrderId,includeProperties: "Order");
                if (order == null || OrderId == null)
                {
                    return BadRequest();
                }
                _response.Result = _mapper.Map<OrderItemDTO>(order);
                _response.Status = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
                
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.IsSuccess = false;
            }
            return _response;
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIRespone>> DeleteProduct(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                var product = await _orderItemRepository.GetAsync(u => u.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
                await _orderItemRepository.DeleteAsync(product);
                _response.Status = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.IsSuccess = false;
            }
            return _response;
        }
        [HttpPut("{id:int}", Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "admin,customer")]

        public async Task<ActionResult<APIRespone>> UpdateProduct(int id, [FromBody] OrderItemDTO dto)
        {
            try
            {
                if (dto == null || id != dto.Id)
                {
                    return BadRequest(dto);
                }

                OrderItem model = _mapper.Map<OrderItem>(dto);
                await _orderItemRepository.UpdateOrderItem(model);
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
    }
}

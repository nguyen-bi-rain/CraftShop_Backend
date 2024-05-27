using System.Net;
using AutoMapper;
using CraftShop.API.Models;
using CraftShop.API.Models.DTO;
using CraftShop.API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace CraftShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{

    private readonly IPaymentRepository _paymentRepository;
    protected APIRespone _response;
    private readonly IMapper _mapper;

    public PaymentController(IPaymentRepository paymentRepository, IMapper mapper)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
        _response = new();
    }

    [HttpGet]
    [Authorize("admin,customer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<APIRespone>> GetOrders(string OrderStatus)
    {
        ICollection<Payment> orders;
        try
        {
            orders = await _paymentRepository.GetAllPayment();
            if (orders == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { "No orders found" };
                return NotFound(_response);
            }
            _response.Result = _mapper.Map<List<PaymentDTO>>(orders);
            _response.Status = HttpStatusCode.OK;
            return Ok(_response);

        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.Message };
        }
        return _response;
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIRespone>> CreatePayment([FromBody] PaymentCreatedDTO dto, string token)
    {
        try
        {
            if (dto == null)
            {
                _response.IsSuccess = false;
                return BadRequest();
            }
            Payment payment = _mapper.Map<Payment>(dto);
            await _paymentRepository.CreatedPayment(payment, token);
            _response.Result = _mapper.Map<PaymentDTO>(payment);
            _response.Status = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {

            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string> { ex.Message };
        }
        return _response;
    }
    [Authorize("admin,customer")]
    [HttpDelete("id:int")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIRespone>> DeletePayment(int? id)
    {
        try
        {

            if (id == null)
            {
                return BadRequest();
            }
            var order = await _paymentRepository.GetPayment(id);
            if (order == null)
            {
                return NotFound();

            }
            await _paymentRepository.DeletePayment(order);
            _response.Status = HttpStatusCode.NoContent;
            return _response;

        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { ex.Message };
        }
        return _response;
    }


}
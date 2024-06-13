using AutoMapper;
using CraftShop.API.Models;
using CraftShop.API.Models.DTO;
using CraftShop.API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CraftShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageRepository _imageRepository;
        private readonly IMapper _mapper;
        private APIRespone _response;

        public ProductImageController(IProductImageRepository imageRepository,IMapper mapper)
        {
            _mapper = mapper;
            _imageRepository = imageRepository;
            this._response = new();
        }
        [Authorize(Roles = "customer,admin")]
        [HttpPost]
        public async Task<ActionResult<APIRespone>> UploadImage([FromForm] ProductImageCreatedDTO dto)
        {
            try
            {
                if(dto == null)
                {
                    _response.IsSuccess = false;
                    _response.Status = System.Net.HttpStatusCode.BadRequest;
                    return BadRequest();
                }
                await _imageRepository.CreateImage(dto);
                
                
            }
            catch (Exception ex) {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
                
            }
            return _response;
        }
        [Authorize(Roles = "customer,admin")]
        [HttpGet]
        public async Task<ActionResult<APIRespone>> GetImageByProductId([FromQuery]int id)
        {
            try
            {
                
                var product = await _imageRepository.GetProductImage(id);
                if(product == null)
                {
                    return NotFound(_response);
                }
                _response.Status = System.Net.HttpStatusCode.OK;
                _response.Result = _mapper.Map<ProductImageDTO>(product) ;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.IsSuccess = false;

            }
            return _response;
        }
    }
}

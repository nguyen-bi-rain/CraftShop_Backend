using AutoMapper;
using CraftShop.API.Models;
using CraftShop.API.Models.DTO;
using CraftShop.API.Repository.IRepository;
using CraftShop.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace CraftShop.API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowOrigin")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // inject cacheService into controller and everytime want get date from database check cache is exist or not if existed 
        // get data from cache, if not get data from database and set data to cache
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private APIRespone _response;
        private readonly ICacheService _cacheService;
        public ProductController(IProductRepository productRepository, IMapper mapper, ICacheService cacheService)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cacheService = cacheService;
            this._response = new();
        }
        [HttpGet("GetAllProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIRespone>> GetAllProduct(int pageSize =3,int pageNumber=1)
        {
            try
            {
                ICollection<Product> products;
                var cacheString =  $"products-{pageSize}-{pageNumber}";
                var cacheData = _cacheService.GetData<ICollection<Product>>(cacheString);
                if (cacheData != null && cacheData.Count > 0)
                {
                    products = cacheData;
                }
                else
                {
                    products = await _productRepository.GetAllAsync(includeProperties: "Category,ProductImage", pageSize: pageSize, pageNumber: pageNumber);
                    _cacheService.SetData(cacheString, products.ToList(), DateTimeOffset.Now.AddMinutes(3));
                }
                var totalProduct = await _productRepository.GetAllNoPagination();
                var pagination = new PaginationFilter(totalProduct.Count, pageSize, pageNumber);
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<ProductDTO>>(products);
                _response.Status = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }
        [HttpGet("{id:int}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIRespone>> GetProduct(int? id)
        {
            try
            {
                if (id == null)
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                Product products = await _productRepository.GetAsync(u => u.Id == id, includeProperties: "ProductImage,Category");
                if (products == null)
                {
                    _response.Status = HttpStatusCode.BadRequest;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ProductDTO>(products);
                _response.Status = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message };
            }
            return _response;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult<APIRespone>> CreateProduct([FromBody] ProductCreateDTO dto)
        {
            try
            {
                if (await _productRepository.GetAsync(u => u.ProductName.ToLower() == dto.ProductName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Product already exists!");
                    return BadRequest(dto);
                }
                if (dto == null)
                {
                    return BadRequest(dto);
                }

                Product model = _mapper.Map<Product>(dto);

                await _productRepository.CreateAsync(model);
                _response.Result = _mapper.Map<ProductDTO>(model);
                _response.Status = HttpStatusCode.Created;
                return CreatedAtRoute("GetProduct", new { id = model.Id }, _response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }
        [HttpDelete("{id:int}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult<APIRespone>> DeleteProduct(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                var product = await _productRepository.GetAsync(u => u.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
                await _productRepository.DeleteAsync(product);
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
        [HttpPut("{id:int}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult<APIRespone>> UpdateProduct(int id, [FromBody] ProductUpdateDTO dto)
        {
            try
            {
                if (dto == null || id != dto.Id)
                {
                    return BadRequest(dto);
                }

                Product model = _mapper.Map<Product>(dto);
                await _productRepository.UpdateProductAsync(model);
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
        [Authorize(Roles = "admin,customer")]
        //search product by price ,category and name.
        [HttpGet("ProductSearch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIRespone>> ProductSearch([FromQuery] ProductSearch query){
            try{
                if(query == null){
                    return BadRequest();
                }
                var products = await _productRepository.SearchProductAsync(query);
                _response.Result = _mapper.Map<List<ProductDTO>>(products);
                _response.Status = HttpStatusCode.OK;
                return Ok(_response);
            }catch(Exception ex){
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }
    }

}

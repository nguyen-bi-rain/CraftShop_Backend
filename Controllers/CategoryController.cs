using AutoMapper;
using CraftShop.API.Models;
using CraftShop.API.Models.DTO;
using CraftShop.API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftShop.API.Controllers
{
    
    [Route("api/category/[controller]")]
    [ApiController]
    
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        protected APIRespone _response;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIRespone>> GetCategoryAsync()
        {
            try
            {
                var cate = await _categoryRepository.GetAllAsync();
                if (cate == null)
                {
                    return NotFound();
                }
                _response.Result = cate;
                _response.Status = System.Net.HttpStatusCode.OK;
                return _response;
            }
            catch (Exception ex)
            {
                _response.ErrorMessages = new List<string>() { ex.Message };
                _response.IsSuccess = false;
                return _response;
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "admin")]

        public async Task<ActionResult<APIRespone>> CreateCategoryAsync([FromBody] CategoryCreatedDTO cate) 
        {
            try
            {
                if(await _categoryRepository.GetAllAsync(u => u.CategoryName.ToLower() == cate.CategoryName.ToLower()) != null){
                    ModelState.AddModelError("ErrorMessage", "Category is already exists!");
                    return BadRequest(_response);
                }
                if (cate == null)
                {
                    return BadRequest();
                }
                Category model = _mapper.Map<Category>(cate);
                await _categoryRepository.CreateAsync(model);
                _response.Status = System.Net.HttpStatusCode.OK;
                return _response;

            }
            catch(Exception ex)
            {
                _response.ErrorMessages = new List<string>() { ex.Message };
                _response.IsSuccess = false;
                return _response;
            }
        }
    }
}

using AutoMapper;
using CraftShop.API.Models;
using CraftShop.API.Models.DTO;
using CraftShop.API.Repository;
using CraftShop.API.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
                var cate = await _categoryRepository.GetAllNoPagination();
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
                if(await _categoryRepository.GetAllNoPagination(u => u.CategoryName.ToLower() == cate.CategoryName.ToLower()) != null){
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
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<APIRespone>> DeleteProduct(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                var product = await _categoryRepository.GetAsync(u => u.Id == id);
                if (product == null)
                {
                    return NotFound();
                }
                await _categoryRepository.DeleteAsync(product);
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
        [HttpPut("{id:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "admin,customer")]

        public async Task<ActionResult<APIRespone>> UpdateProduct(int id, [FromBody] CategoryDTO dto)
        {
            try
            {
                if (dto == null || id != dto.Id)
                {
                    return BadRequest(dto);
                }

                Category model = _mapper.Map<Category>(dto);
                await _categoryRepository.UpdateCategoryAsync(model);
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

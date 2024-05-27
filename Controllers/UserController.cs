using Azure;
using CraftShop.API.Models;
using CraftShop.API.Models.DTO;
using CraftShop.API.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CraftShop.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private APIRespone _response;
        
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            this._response = new();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto)
        {
            var res = await _userRepository.Login(dto);
            if(res == null || string.IsNullOrEmpty(res.Token))
            {
                _response.Status = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Email or password is incorrect");
                return BadRequest(_response);

            }
            _response.Status = HttpStatusCode.OK;
            _response.IsSuccess = true;
            _response.Result = res;
            return Ok(_response);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationDTO dto)
        {
            bool isuservalid = _userRepository.IsUniqueUser(dto.UserName);
            if (!isuservalid)
            {
                _response.Status = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("UserName is already exists");
                return BadRequest(_response);
            }
            var user = await _userRepository.Register(dto);
            if (user == null)
            {
                _response.Status = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("error while register");
                return BadRequest(_response);
            }
            _response.IsSuccess = true;
            _response.Status = HttpStatusCode.OK;
            return Ok(_response);
        }
        
    }
}

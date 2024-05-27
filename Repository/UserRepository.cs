using AutoMapper;
using CraftShop.API.Data;
using CraftShop.API.Models;
using CraftShop.API.Models.DTO;
using CraftShop.API.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;

namespace CraftShop.API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private string SecrectKey;

        public UserRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper,IConfiguration configuration)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            this.SecrectKey = configuration.GetValue<string>("JWT:SecrectKey");
        }

        public string GetUserIdFromToken(string token)
        {
            var tokenHandeler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecrectKey);
            tokenHandeler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            },out SecurityToken validatedToken) ;
            var jwtToken = (JwtSecurityToken)validatedToken;
            string userId = jwtToken.Claims.First(x => x.Type == "unique_name").Value;
            return userId;
        }

        public bool IsUniqueUser(string username)
        {
            var check = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
            if(check != null)
            {
                return false;
            }
            return true;
        }

        public async Task<LoginResponeDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.Email.ToLower() == loginRequestDTO.Email.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user,loginRequestDTO.Password);
            if(user == null || !isValid)
            {
                return new LoginResponeDTO
                {
                    User = null,
                    Token = ""
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecrectKey);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new (new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescription);
            LoginResponeDTO login = new LoginResponeDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDTO>(user),
                Role = roles.FirstOrDefault()
            };
            return login;
        }

        public async Task<UserDTO> Register(RegisterationDTO registerationDTO)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = registerationDTO.UserName,
                Email = registerationDTO.Email,
                PhoneNumber = registerationDTO.PhoneNumber,
                NormalizedEmail = registerationDTO.Email.ToUpper(),
                Name = registerationDTO.UserName
            };
            try
            {
                var result = await _userManager.CreateAsync(user,registerationDTO.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                    await _userManager.AddToRoleAsync(user, "customer");
                    var userReturn = _db.ApplicationUsers.FirstOrDefault(v => user.UserName == registerationDTO.UserName);
                    return _mapper.Map<UserDTO>(userReturn);
                }
                
            }catch (Exception ex)
            {
                
            }
            return new UserDTO();
        }
    }
}

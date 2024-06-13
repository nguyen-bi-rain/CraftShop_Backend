using AutoMapper;
using CraftShop.API.Data;
using CraftShop.API.Models;
using CraftShop.API.Models.DTO;
using CraftShop.API.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public UserRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IConfiguration configuration)
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
            }, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            string userId = jwtToken.Claims.First(x => x.Type == "unique_name").Value;
            return userId;
        }

        public bool IsUniqueUser(string username)
        {
            var check = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == username);
            if (check != null)
            {
                return false;
            }
            return true;
        }

        public async Task<LoginResponeDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.Email.ToLower() == loginRequestDTO.Email.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (user == null || !isValid)
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
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
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
                var result = await _userManager.CreateAsync(user, registerationDTO.Password);
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

            }
            catch (Exception ex)
            {

            }
            return new UserDTO();
        }


        public async Task<UserDTO> GetUserForAccount(string token)
        {
            var userid = GetUserIdFromToken(token);
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userid);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> ChangeUserPassword(string token, string CurrentPassword, string newPassword)
        {
            var userid = GetUserIdFromToken(token);
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userid);
            var result = await _userManager.ChangePasswordAsync(user, CurrentPassword, newPassword);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public async Task UpdateUser(UserDTO userDTO)
        {
            var user = _mapper.Map<ApplicationUser>(userDTO); // Convert UserDTO to ApplicationUser
            await _userManager.UpdateAsync(user);
        }

        public async Task DeleteUser(string userId)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);
            _db.ApplicationUsers.Remove(user);
            await _db.SaveChangesAsync();
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            var user = await _db.ApplicationUsers.ToListAsync();
            var listUser = _mapper.Map<List<UserDTO>>(user);
            return listUser;
        }
        public void DeleteOldUserPhoto(string userId)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId) ?? throw new Exception("User not found.");

            // Check if the file exists at the path stored in the user's PhotoPath property
            if (user.UserPhoto == null)
            {
                user.UserPhoto = "abc";
            }
            var path = Path.Combine("F:\\document\\CraftShop\\frontend\\src\\assets",user.UserPhoto);
            if (File.Exists(path))
            {
                // If the file exists, delete it
                File.Delete(path);
            }
        }
        public async Task ChangeUserPhoto(string token, IFormFile image)
        {
            int maxContent = 1024 * 1024 * 5;
            if (image.Length > maxContent)
            {
                throw new Exception("File size is too large. Please upload a file under 5MB and try again.");
            }
            var userid = GetUserIdFromToken(token);
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userid) ?? throw new Exception("User not found");


            DeleteOldUserPhoto(userid);
            
            var pahtString = Guid.NewGuid().ToString() + "_"+ image.FileName;
            string uploadsFolder = @"F:\document\CraftShop\frontend\src\assets";
            string filePath = Path.Combine(uploadsFolder, pahtString);
            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            user.UserPhoto = pahtString; 
            _db.ApplicationUsers.Update(user);
            _db.SaveChanges();
            
        }

        
    }
}

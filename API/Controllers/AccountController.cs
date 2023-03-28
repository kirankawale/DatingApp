using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entity;
using API.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService tokenService;
        public AccountController(DataContext context,ITokenService tokenService)
        {
            this.tokenService = tokenService;
            _context = context;

        }

        [HttpPost("register")]

        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await IsUserNameExits(registerDTO.UserName))
                return BadRequest("UserName is exits");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDTO.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                SaltPassword = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDTO{
                UserName=user.UserName,
                Token= this.tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> login(LoginDTO loginDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginDTO.UserName.ToLower());

            if (user == null) return Unauthorized("invalid user");

            var hmac = new HMACSHA512(user.SaltPassword);

            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");
            }
            
           return new UserDTO{
                UserName=user.UserName,
                Token= this.tokenService.CreateToken(user)
            };

        }

        private async Task<bool> IsUserNameExits(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
        }

    }
}
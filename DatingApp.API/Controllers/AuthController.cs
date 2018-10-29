using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Data;
using System.Threading.Tasks;
using DatingApp.API.Models;
using DatingApp.API.Dtos;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepositary _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepositary repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegisterDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            userForRegisterDto.username = userForRegisterDto.username.ToLower();

            if (await _repo.UserExist(userForRegisterDto.username))
                return BadRequest("UserName already exist");

            var userdetails = new User
            {
                Username = userForRegisterDto.username
            };

            var CreatedUser = await _repo.Register(userdetails, userForRegisterDto.username);
            return StatusCode(201);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]UserForLoginDto userForLoginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userdetails = await _repo.Login(userForLoginDto.UserName.ToLower(), userForLoginDto.Password);

            if (userdetails == null)
                return Unauthorized();

            var cliams = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier,userdetails.Id.ToString()),
                    new Claim(ClaimTypes.Name,userdetails.Username)
            };

            var key =new  SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

         var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

         var tokenDescriptor = new SecurityTokenDescriptor
         {
             Subject = new ClaimsIdentity(cliams),
             Expires = DateTime.Now.AddDays(1),
             SigningCredentials = creds
         };

         var tokenHandler = new JwtSecurityTokenHandler();

         var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }

    }
}
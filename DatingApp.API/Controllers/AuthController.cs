using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthRepository _authRepo;
    private readonly IConfiguration _config;
    public AuthController(IAuthRepository authRepo, IConfiguration config)
    {
      _config = config;
      _authRepo = authRepo;
    }

    //register
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
    {
      //validate the request
      userForRegisterDTO.Username = userForRegisterDTO.Username.ToLower();
      //check if the username is taken
      if (await _authRepo.UserExists(userForRegisterDTO.Username)) return BadRequest("Username already exists");
      var userToCreate = new User
      {
        Username = userForRegisterDTO.Username
      };

      var createdUser = await _authRepo.Register(userToCreate, userForRegisterDTO.Password);
      //Need to come back and replace with Created at Route
      return StatusCode(201);
    }

    //login
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserForLoginDTO userForLogin)
    {
      var userFromRepo = await _authRepo.Login(userForLogin.Username, userForLogin.Password);
      if (userForLogin == null) return Unauthorized();
      var claims = new[] {
          new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
          new Claim(ClaimTypes.Name, userFromRepo.Username)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings").Value));

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = creds
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return Ok(new
      {
        token = tokenHandler.WriteToken(token)
      });
    }

  }
}

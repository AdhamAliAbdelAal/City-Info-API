using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CityInfoApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CityInfoApi.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : Controller
{
    private class CityInfoUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }

    private CityInfoUser ValidateUserInfo(string? username, string? password)
    {
        // we don't have a user DB or table.  If you have, check the passed-through
        // username/password against what's stored in the database.
        //
        // For demo purposes, we assume the credentials are valid

        // return a new CityInfoUser (values would normally come from your user DB/table)
        return new CityInfoUser()
        {
            Id = 1,
            Username = username ?? string.Empty,
            Name = "John Doe",
            City = "New York"
        };
    }
    
    private string GenerateJwtToken(CityInfoUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]??string.Empty));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>()
        {
            new Claim("sub", user.Username),
            new Claim("id", user.Id.ToString()),
            new Claim("name", user.Name),
            new Claim("city", user.City)
        };
        var token = new JwtSecurityToken(
            _configuration["Authentication:Issuer"],
            _configuration["Authentication:Audience"],
            claims,
            DateTime.Now,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );
        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return tokenString;
    }
    
    private readonly IConfiguration _configuration;
    
    public AuthenticationController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<string>> Login([FromBody] AuthenticationRequestDto request)
    {
        // Step1: validate the user credentials
        var user = ValidateUserInfo(request.Username, request.Password);
        if (user == null)
        {
            return Unauthorized();
        }
        
        // Step2: generate a JWT token
        var token = GenerateJwtToken(user);

        return Ok(token);
    }
    
}
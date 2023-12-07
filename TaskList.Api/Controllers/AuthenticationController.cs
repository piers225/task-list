using Microsoft.AspNetCore.Mvc;
using TaskList.DataAccess.Service;
using TaskList.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TaskList.Api.Controllers;

[ApiController]
[Route("api/authentication")]    
public class AuthenticationController : ControllerBase
{

    private readonly ILogger<AuthenticationController> _logger;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public AuthenticationController(ILogger<AuthenticationController> logger, IUserService userService, IConfiguration configuration)
    {
        _logger = logger;
        _userService = userService;
        _configuration = configuration;
    }

    
    [Route("login")]
    [HttpPost]
    public async Task<ActionResult<LoginSession>> Login([FromBody] LoginRequest loginRequest)
    {
        var hasUser = await _userService.CheckHasUser(loginRequest.Email, loginRequest.Password);

        if (!hasUser) 
        {
            return NotFound();
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "1"),
            new Claim(JwtRegisteredClaimNames.Email, loginRequest.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SECRETKEY"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "your-issuer",
            audience: "your-audience",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.WriteToken(token);
        return Ok(new LoginSession {
            Key = jwt
        });
    }
}
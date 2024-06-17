using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HableChat.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HableChat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(UserManager<User> userManager) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var user = await userManager.FindByEmailAsync(request.email);
        if(user is null)
            return BadRequest();

        bool checkPassword = await userManager.CheckPasswordAsync(user, request.password);
        if(!checkPassword)
            return BadRequest();
        
        var (token, refreshToken) = GetTokens(user);
        user.RefreshToken = refreshToken;
        user.RefreshTokenLifeTime = DateTime.UtcNow.AddMinutes(50);

        await userManager.UpdateAsync(user);
        var response = new LoginResponseDto(token, refreshToken, "OK", checkPassword, user);
        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if(user is not null)
            return BadRequest(new ApiResult("Email já cadastrado", true, null));

        var newUser = new User()
        {
            UserName = request.Username,
            Name = request.Name,
            LastName = request.LastName,
            PasswordHash = request.password,
            Email = request.Email,
        };

        var create = await userManager.CreateAsync(newUser, request.password);
        if(!create.Succeeded)
            return BadRequest(new ApiResult("erro ao criar a conta", true, null));

        return Ok(new ApiResult("Sucesso", false, null));
    }


    private ClaimsPrincipal ReadRefeshToken(string refreshToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                                   .GetBytes("a13e18ab8c8607457f7d8f8a818e731fac47a6099fbeb5e868440666bed9e76a")),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
           

            var principal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters,
                            out SecurityToken securityToken);

            return principal;
    }

    private  Tuple<string, string> GetTokens(User user)
    {
        var myKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a13e18ab8c8607457f7d8f8a818e731fac47a6099fbeb5e868440666bed9e76a"));
        var credentials = new SigningCredentials(myKey, SecurityAlgorithms.HmacSha256);
        var  claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
        };

        var refreshClaim = new[]
        {
            new Claim(ClaimTypes.Name, user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "https://localhost:7266",
            audience: "https://localhost:7266",
            claims: claims,
            expires: DateTime.Now.AddMinutes(1),
            signingCredentials: credentials
        );

        var refrehsToken = new JwtSecurityToken(
            issuer: "https://localhost:7266",
            audience: "https://localhost:7266",
            claims: refreshClaim,
            expires: DateTime.UtcNow.AddMinutes(50),
            signingCredentials: credentials
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshTokenStr = new JwtSecurityTokenHandler().WriteToken(refrehsToken);
        return new Tuple<string, string>(accessToken, refreshTokenStr);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshDto request)
    {
        var claimsPrincipal = ReadRefeshToken(request.refreshToken);
        if(claimsPrincipal is null)
            return BadRequest("Chave inválida");

        var user = await userManager.FindByIdAsync(claimsPrincipal.Identity.Name);

        if(user is null)
            return BadRequest();

        if (user == null || user.RefreshToken != request.refreshToken ||
                       user.RefreshTokenLifeTime <= DateTime.Now)
        {
            return BadRequest("Invalid access token/refresh token");
        }

        var (accessToken, refreshToken) = GetTokens(user);
        user.RefreshToken = refreshToken;
        user.RefreshTokenLifeTime = DateTime.UtcNow.AddMinutes(50);

        await userManager.UpdateAsync(user);

        var response = new RefreshResponseDto(accessToken, refreshToken);
        return Ok(response);
    }


}

public sealed record LoginDto(string email, string password);
public sealed record CreateUserDto(string Username, string Name, string LastName, string Email, string password);
public sealed record LoginResponseDto(string Token, string RefreshToken, string Message, bool status, User user);
public sealed record ApiResult(string message, bool error, object? data);
public sealed record RefreshDto(string refreshToken);
public sealed record RefreshResponseDto(string accessToken ,string refreshToken);
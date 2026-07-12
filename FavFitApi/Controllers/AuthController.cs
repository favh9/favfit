using FavFitApi.Models;
using FavFitApi.Services;
using FavFitApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FavFitApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly FavFitdbContext _context;
    private readonly TokenService _tokenService;
    private readonly IConfiguration _config;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthController(FavFitdbContext context, TokenService tokenService, IConfiguration config, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _tokenService = tokenService;
        _config = config;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] AuthDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
            return Unauthorized("Not authorized to perform that request");

        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        
        if (verificationResult == PasswordVerificationResult.Failed)
            return Unauthorized("Not authorized to perform that request");
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Email, user.Email)
        };

        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = new RefreshToken
        {
            UserId = user.UserId,
            TokenHash = _tokenService.HashToken(_tokenService.GenerateRefreshToken()),
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        var savedRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId.Equals(user.UserId));

        if (savedRefreshToken == null)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
        }
        else
        {
            savedRefreshToken.TokenHash = refreshToken.TokenHash;
            savedRefreshToken.IssuedAt = refreshToken.IssuedAt;
            savedRefreshToken.ExpiresAt = refreshToken.ExpiresAt;
        }
            
        await _context.SaveChangesAsync();

        return Ok( new 
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.TokenHash
            }
        );    
    }

    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh([FromBody] AuthRequestDto request)
    {   
        
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);

        if (principal == null)
            return Unauthorized("Invalid access token");
        
        var savedRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash.Equals(request.RefreshToken));

        if (savedRefreshToken == null || savedRefreshToken.ExpiresAt <= DateTime.UtcNow)
        {
            return Unauthorized("Invalid refresh token");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
        savedRefreshToken.TokenHash = _tokenService.GenerateRefreshToken();
        savedRefreshToken.IssuedAt = DateTime.UtcNow;
        savedRefreshToken.ExpiresAt = DateTime.UtcNow.AddDays(7);

        await _context.SaveChangesAsync();

        return Ok(new
        {
           AccessToken = newAccessToken,
           RefreshToken = savedRefreshToken.TokenHash
        });
        
    }

    
}
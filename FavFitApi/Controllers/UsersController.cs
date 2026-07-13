using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FavFitApi.Data;
using FavFitApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FavFitApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly FavFitdbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UsersController(FavFitdbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        
        if (existingUser != null)
            return Conflict("Email already exists.");

        var newUser = new User()
        {
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            Email = request.Email,
        };
        
        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);
        
        await _context.Users.AddAsync(newUser);

        await _context.SaveChangesAsync();

        var response = new UserDto
        {
            Id = newUser.Id,
            FirstName = newUser.FirstName,
            MiddleName = newUser.MiddleName,
            LastName = newUser.LastName,
            Email = newUser.Email,
            CreatedAt = newUser.CreatedAt,
            ImageUrl = newUser.ImageUrl
        };

        return CreatedAtAction(nameof(GetUserById), new {id = newUser.Id}, response);    
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(long id)
    {  

        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return BadRequest($"Invalid ID");

        var existingUser = await _context.Users.FindAsync(id);

        if (existingUser == null)
            return NotFound($"User with ID {id} not found.");

        if (userId != existingUser.Id.ToString())
            return Unauthorized("Not authorized to perform that request");

        var response = new UserDto
        {
            Id = existingUser.Id,
            FirstName = existingUser.FirstName,
            MiddleName = existingUser.MiddleName,
            LastName = existingUser.LastName,
            Email = existingUser.Email,
            CreatedAt = existingUser.CreatedAt,
            ImageUrl = existingUser.ImageUrl
        };
        
        return response;        
    }

    [Authorize]
    [HttpPatch]
    public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userId == null)
            return BadRequest($"Invalid ID");

        var existingUser = await _context.Users.FindAsync(request.Id);

        if (existingUser == null)
            return NotFound($"User with ID {request.Id} not found.");

        if (userId != existingUser.Id.ToString())
            return Unauthorized("Not authorized to perform that request");

        if (request.NewFirstName != null)
            existingUser.FirstName = request.NewFirstName;

        if (request.NewMiddleName != null)
            existingUser.MiddleName = request.NewMiddleName;

        if (request.NewLastName != null)
            existingUser.LastName = request.NewLastName;

        if (request.NewImageUrl != null)
            existingUser.ImageUrl = request.NewImageUrl;

        if (request.NewEmail != null && request.NewEmail != "")
        {   
            var duplicateUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.NewEmail);

            if (duplicateUser != null)
                return Conflict("Email already exists.");
            
            existingUser.Email = request.NewEmail; 
        }

        if (request.NewPassword != null && request.NewPassword != "")
            existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, request.NewPassword);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(long id)
    {

        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized("Not authorized to perform that request");

        var existingUser = await _context.Users.FindAsync(id);

        if (existingUser == null || userId != existingUser.Id.ToString())
            return Unauthorized("Not authorized to perform that request");
        
        _context.Users.Remove(existingUser);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}
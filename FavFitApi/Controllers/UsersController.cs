using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FavFitApi.Data;
using FavFitApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using System.IO.Pipelines;
using System.ComponentModel;

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
        
        var response = new UserDto
        {
            UserId = newUser.UserId,
            FirstName = newUser.FirstName,
            MiddleName = newUser.MiddleName,
            LastName = newUser.LastName,
            Email = newUser.Email,
            CreatedAt = newUser.CreatedAt,
            ImageUrl = newUser.ImageUrl
        };

        _context.Users.Add(newUser);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserById), new {id = newUser.UserId}, response);    
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(long id)
    {
        
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound($"User with ID {id} not found.");

        var response = new UserDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            ImageUrl = user.ImageUrl
        };
        
        return response;        
    }

    [HttpPatch]
    public async Task<ActionResult> UpdateUser([FromBody] UpdateUserDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var user = await _context.Users.FindAsync(request.UserId);

        if (user == null)
            return NotFound($"User with ID {request.UserId} not found.");

        if (request.NewFirstName != null)
            user.FirstName = request.NewFirstName;

        if (request.NewMiddleName != null)
            user.MiddleName = request.NewMiddleName;

        if (request.NewLastName != null)
            user.LastName = request.NewLastName;

        if (request.NewImageUrl != null)
            user.ImageUrl = request.NewImageUrl;

        if (request.NewEmail != null)
        {   
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.NewEmail);

            if (existingUser != null)
                return Conflict("Email already exists.");
            
            user.Email = request.NewEmail; 
        }

        if (request.NewPassword != null)
            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(long id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound($"User with ID {id} not found.");
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}
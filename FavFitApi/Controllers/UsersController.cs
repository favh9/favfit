using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FavFitApi.Data;
using FavFitApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace FavFitApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly FavFitdbContext _context;

    public UsersController(FavFitdbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var newUser = new User
        {
            FirstName = createUserDto.FirstName,
            MiddleName = createUserDto.MiddleName,
            LastName = createUserDto.LastName,
            Email = createUserDto.Email,
            PasswordHash = createUserDto.PasswordHash
        };

        var userDto = new UserDto
        {
            UserId = newUser.UserId,
            FirstName = newUser.FirstName ?? string.Empty,
            MiddleName = newUser.MiddleName ?? string.Empty,
            LastName = newUser.LastName ?? string.Empty,
            Email = newUser.Email,
            CreatedAt = newUser.CreatedAt,
            ImageUrl = newUser.ImageUrl ?? string.Empty
        };
        
        var res = CreatedAtAction(nameof(GetUserById), new {id = userDto.UserId}, createUserDto); 
        
        _context.Users.Add(newUser);

        await _context.SaveChangesAsync();

        return res;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(long id)
    {
        
        if (id == 0)
            return BadRequest("Enter a valid id");
        
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound($"Id {id}");

        var userDto = new UserDto
        {
            UserId = user.UserId,
            FirstName = user.FirstName ?? string.Empty,
            MiddleName = user.MiddleName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            ImageUrl = user.ImageUrl  ?? string.Empty
        };
        
        return userDto;        
    }

    [HttpPatch]
    public async Task<ActionResult<UserDto>> UpdateUser(UpdateUserDto updateUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (updateUserDto.UserId == 0)
            return BadRequest("Enter a valid id");
        
        var user = await _context.Users.FindAsync(updateUserDto.UserId);

        if (user == null)
            return NotFound($"Id {updateUserDto.UserId}");

        user.FirstName = updateUserDto.FirstName;
        user.MiddleName = updateUserDto.MiddleName;
        user.LastName = updateUserDto.LastName;
        user.ImageUrl = updateUserDto.ImageUrl;

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
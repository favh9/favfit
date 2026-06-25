using System.ComponentModel.DataAnnotations;

namespace FavFitApi.Models;

public class CreateUserDto
{    
    public string? FirstName {get; set;} = null!;
    
    public string? MiddleName {get; set;} = null!;
    
    public string? LastName {get; set;} = null!;

    [Required]
    public string Email {get; set;} = "?";
    
    public string? PasswordHash {get; set;} = null!;
}
using System.ComponentModel.DataAnnotations;

namespace FavFitApi.Models;

public class UpdateUserDto
{    
    [Required]
    public long UserId {get; set;}
    public string? FirstName {get; set;}
    
    public string? MiddleName {get; set;}
    
    public string? LastName {get; set;}

    public string? ImageUrl {get; set;}
}
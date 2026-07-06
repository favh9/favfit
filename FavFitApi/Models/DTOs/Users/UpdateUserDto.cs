using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FavFitApi.Models;

public class UpdateUserDto
{    
    [Required]
    public long UserId {get; set;}
    public string? NewFirstName {get; set;}
    
    public string? NewMiddleName {get; set;}
    
    public string? NewLastName {get; set;}

    public string? NewImageUrl {get; set;}

    public string? NewEmail {get; set;}

    public string? NewPassword {get; set;}
}
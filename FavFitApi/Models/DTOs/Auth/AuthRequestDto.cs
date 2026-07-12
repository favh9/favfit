using System.ComponentModel.DataAnnotations;

namespace FavFitApi.Models;

public class AuthRequestDto
{   
    [Required]
    public string AccessToken {get; set;} = "?";

    [Required]
    public string RefreshToken {get; set;} = null!;
}
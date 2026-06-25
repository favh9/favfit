
using System.ComponentModel.DataAnnotations;

namespace FavFitApi.Models;

public class ChangePasswordDto
{    
    [Required]
    public long UserId {get; set;}

    [Required]
    public string CurrentPassword {get; set;} = "?";

    [Required]
    public string NewPassword {get; set;} = "?";
}
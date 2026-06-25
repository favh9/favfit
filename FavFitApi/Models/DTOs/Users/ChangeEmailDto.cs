
using System.ComponentModel.DataAnnotations;

namespace FavFitApi.Models;

public class ChangeEmailDto
{    
    [Required]
    public long UserId {get; set;}

    [Required]
    public string NewEmail {get; set;} = "?";
}
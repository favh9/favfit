using System.ComponentModel.DataAnnotations;

namespace FavFitApi.Models;

/** read only
 */
public class UserDto
{    
    public long? UserId {get; set;}

    public string? FirstName {get; set;}
    
    public string? MiddleName {get; set;}
    
    public string? LastName {get; set;}

    public string? Email {get; set;}

    public DateTime? CreatedAt {get; set;}

    public string? ImageUrl {get; set;}
}
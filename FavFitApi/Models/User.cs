
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FavFitApi.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("user_id")]
    public long UserId {get; set;}
    
    [Column("first_name")]
    public string? FirstName {get; set;}
    
    [Column("middle_name")]
    public string? MiddleName {get; set;}
    
    [Column("last_name")]
    public string? LastName {get; set;}

    [Required]
    [Column("email")]
    public string Email {get; set;} = "?";

    [Required]
    [Column("password_hash")]
    public string PasswordHash {get; set;} = "?";

    [Column("created_at")]
    public DateTime CreatedAt {get; set;}

    [Column("image_url")]
    public string? ImageUrl {get; set;}
}
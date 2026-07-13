using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FavFitApi.Models;

[Table("refresh_tokens")]
public class RefreshToken
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id {get; set;}

    public User User {get; set;} = null!;

    [ForeignKey(nameof(User))]
    [Column("user_id")]
    public long UserId {get; set;}

    [Column("token_hash")]
    public string TokenHash {get; set;} = "?";

    [Column("issued_at")]
    public DateTime IssuedAt {get; set;}

    [Column("expires_at")]
    public DateTime ExpiresAt {get; set;}
}
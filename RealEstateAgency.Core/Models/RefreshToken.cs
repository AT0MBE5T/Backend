using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateAgency.Core.Models;

[Table("t_refresh_token")]
public class RefreshToken
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("token")]
    public required string Token { get; set; }

    [Column("expires")]
    public DateTime Expires { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("revoked_at")]
    public DateTime? RevokedAt { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public bool IsActive => RevokedAt == null && DateTime.UtcNow <= Expires;
}
using System.ComponentModel.DataAnnotations;

namespace Amsberg_digital_universe.Domain.Common;

public class AuthOptions
{
    [Required]
    public string? Secret { get; set; }

    [Required] public string Audience { get; set; } = null!;
    [Required]
    public int LifetimeInMinutes { get; set; }
    [Required]
    public string Issuer { get; set; } = null!;
}
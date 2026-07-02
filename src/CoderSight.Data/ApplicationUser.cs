using Microsoft.AspNetCore.Identity;

namespace CoderSight.Data;

public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public string? LinkedIn { get; set; }
    public string? Twitter { get; set; }
    public string? Website { get; set; }
}

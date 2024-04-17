using Microsoft.AspNetCore.Identity;

namespace XTracker.Models.Users;
public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}

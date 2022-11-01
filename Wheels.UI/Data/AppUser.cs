using Microsoft.AspNetCore.Identity;

namespace Wheels.UI.Data;
#nullable disable
public class AppUser : IdentityUser
{
    public new string Id {get; set; }
    public string Nickname { get; set; }
    public new string Email { get; set; }
    public string UserAvatar { get; set; }
}
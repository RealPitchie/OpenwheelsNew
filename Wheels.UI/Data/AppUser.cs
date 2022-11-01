using Microsoft.AspNetCore.Identity;

namespace Wheels.UI.Data;
#nullable disable
public class AppUser : IdentityUser
{ 
    public string Nickname { get; set; } 
    public string UserAvatar { get; set; }
}
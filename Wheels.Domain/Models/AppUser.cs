namespace Wheels.Domain.Models;
#nullable disable
public class AppUser 
{
    public string Id {get; set; }
    public string Nickname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } 
}
using Microsoft.AspNetCore.Identity;
using Wheels.Domain.Models;

namespace Wheels.UI.Data;
#nullable disable
public sealed class AppUser : IdentityUser
{ 
    public string Nickname { get; set; } 
    public string UserAvatar { get; set; }
    // public List<string> PostsUpVoted { get; set; }
    // public List<string> PostsDownVoted { get; set; }
    // public List<string> CommentsUpVoted { get; set; }
    // public List<string> CommentsDownVoted { get; set; }
}
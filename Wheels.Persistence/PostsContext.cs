using Microsoft.EntityFrameworkCore;
using Wheels.Domain.Models;
#nullable disable
namespace Wheels.Persistence;

public class PostsContext : DbContext
{
    public PostsContext(DbContextOptions<PostsContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<Comment> Comments { get; set; } 
    public DbSet<Post> Posts { get; set; } 

}
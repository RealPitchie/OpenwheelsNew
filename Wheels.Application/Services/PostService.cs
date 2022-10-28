using Microsoft.EntityFrameworkCore;
using Wheels.Application.Interfaces;
using Wheels.Domain.Models;
using Wheels.Persistence;
#nullable disable
namespace Wheels.Application.Services;

public class PostService : IPostService
{
    private readonly PostsContext _context;

    public PostService(PostsContext context)
    {
        _context = context;
    }
    public async Task AddCommentAsync(Comment comment)
    {
        await _context.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task AddPostAsync(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task<Post> EditPostAsync(Post post)
    {
        throw new NotImplementedException();
    }

    public async Task<Comment> EditCommentAsync(Comment comment)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Post>> GetPostsAsync(int pageNo)
    {
        return await _context.Posts
            .OrderByDescending(p => p.Posted)
            .Skip(pageNo * 10)
            .Take(10)
            .ToListAsync();
    }

    public async Task<Post> GetPostAsync(string postId)
    {
        return await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
    }

    public async Task<List<Comment>> GetCommentsByPostIdAsync(string targetPostId)
    {
        throw new NotImplementedException();
    }

    public int GetPostsCount()
    {
        return _context.Posts.Count();
    }
}
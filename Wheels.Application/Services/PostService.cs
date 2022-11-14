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
    public async Task AddCommentAsync(Post post)
    {
        _context.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task AddPostAsync(Post post)
    {
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePost(Post post)
    {
        post.WasDeleted = true;
        _context.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task  EditPostAsync(Post post)
    {
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task<Comment> EditCommentAsync(Comment comment)
    {
        _context.Update(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<List<Post>> GetPostsAsync(int pageNo)
    {
        return await _context.Posts
            .Include(p => p.Votes)
            .Where(p => !p.WasDeleted)
            .OrderByDescending(p => p.Posted)
            .Skip(pageNo * 10)
            .Take(10)
            .ToListAsync();
    }

    public async Task<List<Post>> GetPostsByWeeks(int weeksToShow, int pageNo)
    {
        return await _context.Posts
            .Include(p => p.Votes)
            .Where(p => p.Posted >= DateTime.Now.AddDays(weeksToShow * 7))
            .OrderByDescending(p => p.Posted)
            .Skip(pageNo *10)
            .Take(10)
            .ToListAsync();
    }

    public async Task<List<Post>> GetPostsByRatingAsync()
    {
        return await _context.Posts
            .OrderByDescending(p => p.Rating)
            .Take(10)
            .ToListAsync();
    }

    public async Task<Post> GetPostAsync(string postId)
    {
        return await _context.Posts
            .Include(p => p.Votes)
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == postId);
    }

    public async Task<List<Comment>> GetCommentsByPostIdAsync(string targetPostId)
    {
        var post = await _context.Posts.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == targetPostId);
        return post != null ? post.Comments : new List<Comment>();
    }

    public int GetPostsCount()
    {
        return _context.Posts.Count();
    }

    public Dictionary<string, string> GetPostsTitles()
    {
        return _context.Posts.Select(x =>
            new KeyValuePair<string, string>(x.Id, x.Title))
            .ToDictionary(x => x.Key, x => x.Value);
    }

    public async Task<int> DbCleanup()
    {
        var duplicates = _context.Posts.AsEnumerable().GroupBy(a => new { a.Title})
            .Where(a => a.Count() > 1)
            .SelectMany(a => a.ToList());

        _context.RemoveRange(duplicates);

        await _context.SaveChangesAsync();
        return duplicates.Count();
    }

    
}
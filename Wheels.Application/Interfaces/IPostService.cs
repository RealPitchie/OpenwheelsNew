using Wheels.Domain.Models;

namespace Wheels.Application.Interfaces;

public interface IPostService
{
    Task AddCommentAsync(Comment comment);
    Task AddPostAsync(Post post);
    Task<Post> EditPostAsync(Post post);
    Task<Comment> EditCommentAsync(Comment comment);
    Task<List<Post>> GetPostsAsync();
    Task<Post> GetPostAsync();
    Task<List<Comment>> GetCommentsByPostIdAsync(string targetPostId);
}
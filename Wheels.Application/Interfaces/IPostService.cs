using Wheels.Domain.Models;

namespace Wheels.Application.Interfaces;

public interface IPostService
{
    Task AddCommentAsync(Comment comment);
    Task AddPostAsync(Post post);
    Task<Post> EditPostAsync(Post post);
    Task<Comment> EditCommentAsync(Comment comment);
    Task<List<Post>> GetPostsAsync(int pageNo);
    Task<Post> GetPostAsync(string postId);
    Task<List<Comment>> GetCommentsByPostIdAsync(string targetPostId);
}
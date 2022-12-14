using Wheels.Domain.Models;

namespace Wheels.Application.Interfaces;

public interface IPostService
{
    Task AddCommentAsync(Post post);
    Task AddPostAsync(Post post);
    Task DeletePost(Post post);
    Task EditPostAsync(Post post);
    Task<Comment> EditCommentAsync(Comment comment);
    Task<List<Post>> GetPostsByCount(int offset);
    Task<List<Post>> GetPostsAsync(int pageNo); 
    Task<List<Post>> GetPostsByWeeks(int weeksToShow, int pageNo);
    Task<List<Post>> GetPostsByRatingAsync();
    Task<Post> GetPostAsync(string postId);
    Task<List<Comment>> GetCommentsByPostIdAsync(string targetPostId);
}
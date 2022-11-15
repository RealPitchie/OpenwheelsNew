using Wheels.Application.Interfaces;
using Wheels.Domain.Models;
using Wheels.Persistence;

namespace Wheels.Application.Services;

public class VoteService : IVoteservice
{
    private readonly PostsContext _context;

    public VoteService(PostsContext context)
    {
        _context = context;
    }

    public async Task Vote(Post post, string? userId, string vote)
    {
        var newVote = new Vote() { Id = Guid.NewGuid().ToString(), VoteType = vote, User = userId };
        switch (vote)
        {
            case "up":
                post.Votes.Remove(post.Votes.Find(v => v.User == userId && v.VoteType == "down"));
                post.Rating++;
                break;
            case "down":
                post.Votes.Remove(post.Votes.Find(v => v.User == userId && v.VoteType == "up"));
                post.Rating--;
                break;
        }
        post.Votes.Add(newVote);
        await _context.AddAsync(newVote);
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public bool WasUpvotedByUser(Post post, string userId)
    { 
        return post.Votes.Any(v => v.User == userId && v.VoteType == "up");
    }

    public  bool WasDownvotedByUser(Post post, string userId)
    { 
        return post.Votes.Any(v => v.User == userId && v.VoteType == "down");
    } 
}
using Wheels.Domain.Models;

namespace Wheels.Application.Interfaces;

public interface IVoteservice
{
    Task Vote(Post post, string? userId, string vote);
    bool WasUpvotedByUser(Post post, string userId);
    bool WasDownvotedByUser(Post post, string userId);
}
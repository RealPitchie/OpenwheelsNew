namespace Wheels.Domain.Models;

public class Vote
{
    public string Id { get; set; }
    public string VoteType { get; set; }
    public string? User { get; set; }
}
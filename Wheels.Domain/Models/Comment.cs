namespace Wheels.Domain.Models;
#nullable disable
public class Comment
{
    public string Id { get; set; }
    public string Author { get; set; } 
    public string Text { get; set; }
    public DateTime Posted { get; set; } 
    public string PostId { get; set; }
    public string ImageFile { get; set; } 
    public List<Vote> Votes { get; set; }
}
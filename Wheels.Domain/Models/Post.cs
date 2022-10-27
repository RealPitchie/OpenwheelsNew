namespace Wheels.Domain.Models;
#nullable disable
public class Post
{
    public string Id { get; set; }
    public DateTime Posted { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public string[] Text { get; set; }
    public string[] ImageFile { get; set; } = new string[10];
    public string VideoLink { get; set; }
         
    public int Rating { get; set; }
         
    public string Author { get; set; }
    public string AuthorAvatar { get; set; } 
        
}
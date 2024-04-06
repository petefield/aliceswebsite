using AlicesWebsite.Shared.Accounts;
using static AlicesWebsite.Client.Pages.Index;

namespace AlicesWebsite.Server.Models;

public class Comment
{
    public Comment()
    {
        Id = Guid.Empty;
        UserId = Guid.Empty;
        Posted = DateTime.MinValue;
        Content = string.Empty;
        VideoId = Guid.Empty;
    }
    public Comment(Guid id, Guid userId, DateTime posted, string content, Guid videoId)
    {
        Id = id;
        UserId = userId;
        Posted = posted;
        Content = content;
        VideoId = videoId;
    }
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Posted { get; set; }
    public string Content { get; set; }
    public Guid VideoId { get; set; }
}

using AlicesWebsite.Shared.Accounts;


namespace AlicesWebsite.Shared
{
    public record CommentResponse (Guid Id, User user, string message, Guid videoId, DateTime posted);
}

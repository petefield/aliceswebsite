namespace AlicesWebsite.Shared;

public record class CommentRequest(Guid VideoId, string Content);

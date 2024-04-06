namespace AlicesWebsite.Server.Models;

public record Video(Guid Id, Guid UploadedBy, DateTime UploadedAt, string Description, string Thumbnail, string Url);
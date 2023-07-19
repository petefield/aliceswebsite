using AlicesWebsite.Server.Models;
using OneOf;
using OneOf.Types;

namespace AlicesWebsite.Server.Data;

public interface ICommentsRepo
{
    Task<IEnumerable<Comment>> GetComments();
    Task<Comment> AddComment(Comment comment);
    Task<OneOf<Success, Error>> DeleteComment(Comment comment);
    Task<OneOf<Comment, NotFound , Error>> GetComment(Guid id);
}
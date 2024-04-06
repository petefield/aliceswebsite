using AlicesWebsite.Server.Models;
using AlicesWebsite.Shared;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace AlicesWebsite.Server.Data
{
    public class CommentsRepo : ICommentsRepo
    {
        private readonly CommentsContext _commentsContext;

        public CommentsRepo(CommentsContext commentsContext)
        {
            _commentsContext = commentsContext;
        }

        public async Task<Comment> AddComment(Comment comment)
        {
            _commentsContext.Comments.Add(comment); 
            await _commentsContext.SaveChangesAsync();
            return comment;
        }

        public async Task<IEnumerable<Comment>> GetComments()
        {
            return await _commentsContext.Comments.ToListAsync();
        }

        public async Task<OneOf<Success,Error>> DeleteComment(Comment comment)
        {
            _commentsContext.Remove(comment);
            await _commentsContext.SaveChangesAsync();
            return new Success();
        }

        public async Task<OneOf<Comment, NotFound, Error>> GetComment(Guid id)
        {
            var comment = await _commentsContext.Comments.FindAsync(id);

            if(comment == null) { return new NotFound(); }
            
            return comment;
        }
    }
}

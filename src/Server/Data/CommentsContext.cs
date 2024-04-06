using AlicesWebsite.Server.Models;
using AlicesWebsite.Shared;
using Microsoft.EntityFrameworkCore;

namespace AlicesWebsite.Server.Data
{
    public class CommentsContext : DbContext
    {
        public CommentsContext(DbContextOptions<CommentsContext> options) : base(options)
        {
        }

        protected CommentsContext()
        {
        }

        public DbSet<Comment> Comments { get; set; }
    }
}

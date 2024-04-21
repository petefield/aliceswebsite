using AlicesWebsite.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace AlicesWebsite.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected DataContext()
        {
        }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Video> Videos { get; set; }

    }
}

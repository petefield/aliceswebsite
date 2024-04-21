using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlicesWebsite.Server.Data
{
    public class IdentityContext : IdentityDbContext
    {
        public IdentityContext()
        {

        }

        public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

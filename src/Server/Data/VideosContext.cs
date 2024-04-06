using AlicesWebsite.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace AlicesWebsite.Server.Data;

public class VideosContext : DbContext
{
    public VideosContext(DbContextOptions<VideosContext> options) : base(options)
    {
    }

    protected VideosContext()
    {
    }

    public DbSet<Video> Videos { get; set; }
}

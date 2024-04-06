using AlicesWebsite.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace AlicesWebsite.Server
{
    public static class DependancyInjectionExtensions
    {
        public static IServiceCollection AddComments(this IServiceCollection services, string? commentsDb)
        {
            if (string.IsNullOrEmpty(commentsDb))
                throw new ArgumentNullException(nameof(commentsDb));

            services.AddDbContext<CommentsContext>(opt => opt.UseSqlServer(commentsDb));
            return services.AddScoped<ICommentsRepo, CommentsRepo>();
        }


        public static IServiceCollection AddVideos(this IServiceCollection services, string? videosDb)
        {
            if (string.IsNullOrEmpty(videosDb))
                throw new ArgumentNullException(nameof(videosDb));

            services.AddDbContext<VideosContext>(opt => opt.UseSqlServer(videosDb));
            return services.AddScoped<IVideoRepo, VideosRepo>();
        }
    }
}

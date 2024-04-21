using AlicesWebsite.Server.Data;

namespace AlicesWebsite.Server
{
    public static class DependancyInjectionExtensions
    {
        public static IServiceCollection AddComments(this IServiceCollection services)
        {
            return services.AddScoped<ICommentsRepo, CommentsRepo>();
        }

        public static IServiceCollection AddVideos(this IServiceCollection services)
        {
            return services.AddScoped<IVideoRepo, VideosRepo>();
        }
    }
}

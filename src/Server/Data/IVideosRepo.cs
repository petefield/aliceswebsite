using AlicesWebsite.Server.Models;
using OneOf;
using OneOf.Types;

namespace AlicesWebsite.Server.Data;

public interface IVideoRepo
{
    Task<IEnumerable<Video>> GetVideos();
    Task<Video> AddVideo(Video comment);
    Task<OneOf<Success, Error>> DeleteVideo(Video comment);
    Task<OneOf<Video, NotFound , Error>> GetVideo(Guid id);
}
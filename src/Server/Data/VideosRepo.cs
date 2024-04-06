using AlicesWebsite.Server.Models;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace AlicesWebsite.Server.Data
{
    public class VideosRepo : IVideoRepo
    {
        private readonly VideosContext _context;

        public VideosRepo(VideosContext context)
        {
            _context = context;
        }

        public async Task<Video> AddVideo(Video comment)
        {
            _context.Videos.Add(comment); 
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<IEnumerable<Video>> GetVideos()
        {
            return await _context.Videos.ToListAsync();
        }

        public async Task<OneOf<Success,Error>> DeleteVideo(Video comment)
        {
            _context.Remove(comment);
            await _context.SaveChangesAsync();
            return new Success();
        }

        public async Task<OneOf<Video, NotFound, Error>> GetVideo(Guid id)
        {
            var video = await _context.Videos.FindAsync(id);

            if(video == null) { return new NotFound(); }
            
            return video;
        }
    }
}

using AlicesWebsite.Server.Data;
using AlicesWebsite.Server.Models;
using AlicesWebsite.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace AlicesWebsite.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IVideoRepo _repo;

        public VideosController(IVideoRepo repo)
        {
            _repo = repo;
        }

        // GET: api/<CommentsController>
        [HttpGet]
        public async Task<IEnumerable<GetVideoResponse>> Get()
        {
            var videos = await _repo.GetVideos();
            var responses = videos.Select(video => new GetVideoResponse(video.Id, video.Thumbnail, video.Description));
            return responses;
        }


        // POST api/<CommentsController>
        [HttpPost]
        [Authorize]
        public async Task<CreateVideoResponse> Post(CreateVideoRequest request)
        {
            var userId = this.GetUserId();
            var video = new Video(Guid.NewGuid(), userId, DateTime.UtcNow, request.Description, request.Thumbnail, request.Description);
            var addVideoResult = await _repo.AddVideo(video);
            return new CreateVideoResponse(video.Id, video.Thumbnail, video.Description); ;
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(Guid id)
        {
            var userId = this.GetUserId();

            var commentResult = await _repo.GetVideo(id);

            var response = await commentResult.Match<Task<ActionResult>>(
                 async video =>
                 {
                     if (video.UploadedBy != userId)
                         return Forbid();
         
                     var deleteResult = await _repo.DeleteVideo(video);

                     return deleteResult.Match<ActionResult>(
                         success => Ok(),
                         _ => Problem()
                     );
                 },
                NotFound => Task.FromResult(this.NotFound() as ActionResult),
                _ => Task.FromResult(this.Problem() as ActionResult)
                );

            return response;
        }
    }
}

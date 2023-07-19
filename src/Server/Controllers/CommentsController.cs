using AlicesWebsite.Server.Data;
using AlicesWebsite.Server.Models;
using AlicesWebsite.Shared;
using AlicesWebsite.Shared.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneOf.Types;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AlicesWebsite.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsRepo _commentsRepo;
        private readonly UserManager<IdentityUser> _userManager;

        public CommentsController(ICommentsRepo commentsRepo, UserManager<IdentityUser> userManager)
        {
            _commentsRepo = commentsRepo;
            _userManager = userManager;
        }

        // GET: api/<CommentsController>
        [HttpGet]
        public async Task<IEnumerable<CommentResponse>> Get()
        {
            var comments = await _commentsRepo.GetComments();
                
            var commentResponses = new List<CommentResponse>();   
           
            foreach (var comment in comments) {
                commentResponses.Add( await Map(comment, await _userManager.FindByIdAsync(comment.UserId.ToString())));
            }

            return commentResponses;
        }


        // POST api/<CommentsController>
        [HttpPost]
        [Authorize]
        public async Task<CommentResponse> Post(CommentRequest request)
        {
            var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var uId) ? uId : Guid.Empty;

            var comment = new Comment(
                Guid.NewGuid(),
                userId,
                DateTime.UtcNow, request.Content, request.VideoId);
            var addCommentResult =  await _commentsRepo.AddComment(comment); 

            return await Map(addCommentResult, await _userManager.FindByIdAsync(userId.ToString()));
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(Guid id)
        {
            Guid commentId = id;

            var userId = Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var uId) ? uId : Guid.Empty;

            var commentResult = await _commentsRepo.GetComment(commentId);

            var response = await commentResult.Match<Task<ActionResult>>(
                 async comment =>
                 {
                     if (comment.UserId != userId) {
                         return Forbid();
                     }

                     var deleteResult = await _commentsRepo.DeleteComment(comment);

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


        private async Task<CommentResponse> Map(Comment c, IdentityUser? u)
        {
            var claims = u is null
                ? new List<Claim>()
                : await _userManager.GetClaimsAsync(u);

            var user = new User(
                u?.UserName ?? string.Empty,
                u?.Email ?? string.Empty,
                claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value ?? "-",
                claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value ?? "-");


            return new CommentResponse(
                c.Id,
                user,
                c.Content,
                c.VideoId,
                c.Posted);
        }

        // PUT api/<CommentsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

    }
}

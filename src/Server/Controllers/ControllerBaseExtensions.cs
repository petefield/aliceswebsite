using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AlicesWebsite.Server.Controllers
{
    public static class ControllerBaseExtensions
    {


        public static Guid GetUserId(this ControllerBase source) {
            return Guid.TryParse(source.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var uId) ? uId : Guid.Empty;
        }   

    }
}

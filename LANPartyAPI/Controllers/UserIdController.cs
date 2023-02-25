using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace LANPartyAPI.Controllers
{
    public abstract class UserIdController : ControllerBase
    {
        /// <summary>
        /// Only use this method in an action with [Authorize] attribute
        /// </summary>
        /// <returns></returns>
        protected string GetUserIdWhenAuthorize()
        {
            var userId = this.User.Claims.FirstOrDefault(i => i.Type == "user_id");
            return userId != null ? userId.Value : null;
        }

        /// <summary>
        /// You can use this method in any action to get the logged in user (if logged in)
        /// </summary>
        /// <returns></returns>
        protected async Task<string> GetUserIdAsync()
        {
            var userId = "";
            var auth = await HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (auth.Succeeded)
            {
                var claimsPrincipal = auth.Principal;
                //var subject = claimsPrincipal.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier);
                var value = claimsPrincipal.Claims.FirstOrDefault(i => i.Type == "user_id");
                userId = value != null ? value.Value : "";
                // use the subject claim as needed (actual value is "subject.Value")
            }
            return userId;
        }
    }
}

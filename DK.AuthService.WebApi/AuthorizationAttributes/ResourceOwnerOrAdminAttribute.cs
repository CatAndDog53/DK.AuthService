using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DK.AuthService.WebApi.AuthorizationAttributes
{
    public class ResourceOwnerOrAdminAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var usernameClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (usernameClaim == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var routeData = context.RouteData.Values;
            var username = routeData["username"]?.ToString();

            if (usernameClaim != username && !user.IsInRole("Admin"))
            {
                context.Result = new ForbidResult();
            }
        }
    }

}

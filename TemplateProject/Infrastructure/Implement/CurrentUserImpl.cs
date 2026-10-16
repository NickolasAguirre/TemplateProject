using Microsoft.AspNetCore.Http;
using TemplateProject.Application.Services.GeneralServices.CurrentUserService;

namespace TemplateProject.API.CurrentUserImpl
{
    public class CurrentUserImplementation(IHttpContextAccessor httpContextAccessor) : CurrentUserService
    {
        public override string? UserId { get; set; }
        public override string? UserName { get; set; }
        public override void SetCurrentUser()
        {
            var identity = httpContextAccessor.HttpContext?.User?.Identity;
            if (identity != null && identity.IsAuthenticated)
            {
                UserId = identity.Name ?? "Unknown";
                UserName = identity.Name ?? "Unknown";
            }
            else
            {
                UserId = "Unknown";
                UserName = "Unknown";
            }
        }

    }
}

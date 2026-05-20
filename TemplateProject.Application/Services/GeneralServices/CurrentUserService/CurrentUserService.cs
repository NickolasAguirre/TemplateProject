using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateProject.Application.Services.GeneralServices.CurrentUserService
{
    public abstract class CurrentUserService
    {
        public abstract string? UserId { get; set; }
        public abstract string? UserName { get; set; }
        public abstract void SetCurrentUser();
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TemplateProject.Application.GeneralServices.CurrentUserService;

namespace TemplateProject.Infrastructure.DatabaseContext
{
    public class TemplateProjectDbContext
    {
        private readonly CurrentUserService _currentUserService;

        public TemplateProjectDbContext(DbContextOptions<TemplateProjectDbContext> options, CurrentUserService currentUserService): base(options)
        {
            _currentUserService = currentUserService;
        }
    }
}

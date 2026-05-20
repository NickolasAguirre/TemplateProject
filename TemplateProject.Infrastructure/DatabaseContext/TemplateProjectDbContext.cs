using Microsoft.EntityFrameworkCore;
//#if (userTracking)
using TemplateProject.Application.Services.GeneralServices.CurrentUserService;
using TemplateProject.Domain.Entities.CommonEntities;
//#endif

namespace TemplateProject.Infrastructure.DatabaseContext
{
    public class TemplateProjectDbContext : DbContext
    {
//#if (userTracking)
        private readonly CurrentUserService _currentUserService = null!;

        public TemplateProjectDbContext(DbContextOptions<TemplateProjectDbContext> options, CurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _currentUserService.SetCurrentUser();
            var userId = _currentUserService.UserId ?? "Unknown";

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.SetCreatedAuditInfo(userId);
                else if (entry.State == EntityState.Modified)
                    entry.Entity.SetUpdatedAuditInfo(userId);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
//#else
        public TemplateProjectDbContext(DbContextOptions<TemplateProjectDbContext> options) : base(options)
        {
        }
//#endif
    }
}

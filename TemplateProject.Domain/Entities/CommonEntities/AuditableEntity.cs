using System;
using System.Collections.Generic;
using System.Text;

namespace TemplateProject.Domain.Entities.CommonEntities
{
    public abstract class AuditableEntity
    {
        public string TenantId { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool State { get; set; }
        public string CreatedBy { get; protected set; } = string.Empty;
        public DateTime CreatedAt { get; protected set; }
        public string? UpdatedBy { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }

        public void SetCreatedAuditInfo(string userId)
        {
            CreatedBy = userId;
            CreatedAt = DateTime.UtcNow;
        }
        public void SetUpdatedAuditInfo(string userId)
        {
            UpdatedBy = userId;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
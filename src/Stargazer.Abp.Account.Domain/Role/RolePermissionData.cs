using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace Stargazer.Abp.Account.Domain.Role
{
    public class RolePermissionData : AuditedAggregateRoot<Guid>
    {
        public RolePermissionData()
        {
        }

        public RolePermissionData(Guid id, Guid roleId, Guid permissionId) : base(id)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }

        public Guid RoleId { get; set; }

        public Guid PermissionId { get; set; }

        public PermissionData PermissionData { get; set; }
    }
}
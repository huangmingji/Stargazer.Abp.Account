using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Stargazer.Abp.Account.Domain.Role
{
    public class RolePermissionData : Entity<Guid>
    {
        public RolePermissionData()
        {
        }

        public RolePermissionData(Guid id, Guid roleId, string permission) : base(id)
        {
            Check.NotNullOrWhiteSpace(permission, nameof(permission));
            RoleId = roleId;
            Permission = permission;
        }

        public Guid RoleId { get; set; }

        public string Permission { get; set; }
    }
}
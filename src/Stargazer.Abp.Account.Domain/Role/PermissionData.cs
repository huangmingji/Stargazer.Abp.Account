using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Stargazer.Abp.Account.Domain.Role
{
    /// <summary>
    /// 权限
    /// </summary>
    public sealed class PermissionData : AuditedEntity<Guid>
    {
        public PermissionData()
        {
        }

        public PermissionData(Guid id, string name, string permission, Guid? parentId = null)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(permission, nameof(permission));
            Id = id;
            ParentId = parentId;
            Name = name;
            Permission = permission;
        }

        public void Set(string name, string permission, Guid? parentId = null)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(permission, nameof(permission));
            ParentId = parentId;
            Name = name;
            Permission = permission;
        }

        /// <summary>
        /// 上级主键
        /// </summary>
        public Guid? ParentId { get; set; }
        
        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 权限
        /// </summary>
        public string Permission { get; set; }
    }
}
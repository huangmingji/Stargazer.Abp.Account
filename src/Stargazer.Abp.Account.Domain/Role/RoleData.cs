using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Stargazer.Abp.Account.Domain.Role
{
    /// <summary>
    /// 角色表
    /// </summary>
    public sealed class RoleData : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public RoleData()
        {
        }
        
        public RoleData(Guid id, string name, bool isDefault, bool isStatic, bool isPublic) : base(id)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Name = name;
            IsDefault = isDefault;
            IsStatic = isStatic;
            IsPublic = isPublic;
        }

        public Guid? TenantId { get; set; }
        
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 默认角色自动分配给新用户
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 静态角色无法删除/重命名
        /// </summary>
        /// <returns></returns>
        public bool IsStatic { get; set; }

        /// <summary>
        /// 允许其他用户查看，一般只有超管才分配为 false 的角色
        /// </summary>
        /// <returns></returns>
        public bool IsPublic { get; set; }

        public List<RolePermissionData> Permissions { get; set; } = new List<RolePermissionData>();

    }
}
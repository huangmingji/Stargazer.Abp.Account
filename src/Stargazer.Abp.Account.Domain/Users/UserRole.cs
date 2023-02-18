using System;
using Stargazer.Abp.Account.Domain.Role;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Stargazer.Abp.Account.Domain.Users
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class UserRole : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public UserRole()
        {
        }

        public UserRole(Guid id, Guid userId, Guid roleId) : base(id)
        {
            UserId = userId;
            RoleId = roleId;
        }

        /// <summary>
        /// 租户主键
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 用户主键
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// 角色主键
        /// </summary>
        public Guid RoleId { get; set; }

        public RoleData RoleData { get; set; }
    }
}
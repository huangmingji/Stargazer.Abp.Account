using System;
using Stargazer.Abp.Account.Application.Contracts.Roles.Dtos;

namespace Stargazer.Abp.Account.Application.Contracts.Users.Dtos
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class UserRoleDto
    {
        public Guid Id { get; set; }

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

        public RoleDto RoleData { get; set; } = new RoleDto();
    }
}
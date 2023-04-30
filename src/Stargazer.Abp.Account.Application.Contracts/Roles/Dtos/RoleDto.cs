using System;
using System.Collections.Generic;

namespace Stargazer.Abp.Account.Application.Contracts.Roles.Dtos
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        
        public Guid? TenantId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; } = "";

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

        public List<RolePermissionDto> Permissions { get; set; } = new List<RolePermissionDto>();

    }
}
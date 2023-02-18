using System;
using System.Collections.Generic;

namespace Stargazer.Abp.Account.Application.Contracts.Permissions.Dtos
{
    public class PermissionDto
    {
        public Guid Id { get; set; }
        
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

        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }
}
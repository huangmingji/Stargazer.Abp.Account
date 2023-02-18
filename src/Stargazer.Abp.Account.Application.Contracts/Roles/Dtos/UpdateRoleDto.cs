using System.Collections.Generic;

namespace Stargazer.Abp.Account.Application.Contracts.Roles.Dtos
{
    public class UpdateRoleDto
    {
        public string Name { get; set; }
        
        /// <summary>
        /// 默认角色自动分配给新用户
        /// </summary>
        public bool IsDefault { get; set; }

        public List<string> Permissions { get; set; } = new List<string>();
    }
}
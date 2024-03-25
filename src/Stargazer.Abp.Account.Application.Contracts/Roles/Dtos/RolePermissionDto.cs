using System;
using Stargazer.Abp.Account.Application.Contracts.Permissions.Dtos;

namespace Stargazer.Abp.Account.Application.Contracts.Roles.Dtos
{
    public class RolePermissionDto 
    {

        public Guid Id { get; set; }
        
        public Guid RoleId { get; set; }

        public Guid PermissionId { get; set; }

        public PermissionDto PermissionData { get; set; }
    }
}
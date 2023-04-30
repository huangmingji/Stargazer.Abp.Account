using System;

namespace Stargazer.Abp.Account.Application.Contracts.Permissions.Dtos
{
    public class UpdatePermissionDto
    {
        public Guid? ParentId { get; set; }
        
        public string Name { get; set; } = "";
        
        public string Permission { get; set; } = "";
    }
}
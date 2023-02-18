using System;

namespace Stargazer.Abp.Account.Application.Contracts.Roles.Dtos
{
    public class RolePermissionDto 
    {

        public Guid Id { get; set; }
        
        public Guid RoleId { get; set; }

        public string Permission { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace Stargazer.Abp.Account.Application.Contracts.Users.Dtos
{
    public class UpdateUserRoleDto
    {
        public List<Guid> RoleIds { get; set; } = new List<Guid>();
    }
}
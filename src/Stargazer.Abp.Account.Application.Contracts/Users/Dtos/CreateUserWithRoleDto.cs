using System;
using System.Collections.Generic;

namespace Stargazer.Abp.Account.Application.Contracts.Users.Dtos
{
    public class CreateUserWithRolesDto : CreateUserDto
    {
        public string Account { get; set; }
        
        public string PhoneNumber { get; set; }

        public List<Guid> RoleIds { get; set; } = new List<Guid>();
    }
}
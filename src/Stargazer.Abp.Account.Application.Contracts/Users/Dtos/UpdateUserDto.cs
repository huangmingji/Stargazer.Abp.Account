using System;
using System.Collections.Generic;

namespace Stargazer.Abp.Account.Application.Contracts.Users.Dtos
{
    public class UpdateUserDto
    {
        public string Name { get; set; }
        
        public string Account { get; set; }
        
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        
        public string Password { get; set; }

        public List<Guid> RoleIds { get; set; }
    }
}
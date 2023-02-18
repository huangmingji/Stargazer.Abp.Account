using System;

namespace Stargazer.Abp.Account.Application.Contracts.Users.Dtos
{
    public class CreateUserWithRoleDto : CreateUserDto
    {
        public Guid RoleId { get; set; }
    }
}
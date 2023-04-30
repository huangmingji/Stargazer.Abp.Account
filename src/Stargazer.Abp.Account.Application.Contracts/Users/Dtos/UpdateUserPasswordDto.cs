using System;

namespace Stargazer.Abp.Account.Application.Contracts.Users.Dtos
{
    public class UpdateUserPasswordDto : UpdatePasswordDto
    {
        public string OldPassword { get; set; } = "";
        
    }
}
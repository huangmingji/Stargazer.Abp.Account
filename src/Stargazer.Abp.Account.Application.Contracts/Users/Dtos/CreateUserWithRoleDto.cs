using System;
using System.Collections.Generic;

namespace Stargazer.Abp.Account.Application.Contracts.Users.Dtos
{
    public class CreateUserWithRolesDto : CreateUserDto
    {
        public string Account { get; set; } = "";

        /// <summary>
        /// 电子邮箱是否已验证
        /// </summary>
        /// <value></value>
        public bool EmailVerified { get; set; }
        
        public string PhoneNumber { get; set; } = "";
        
        /// <summary>
        /// 手机号码是否已验证
        /// </summary>
        /// <value></value>
        public bool PhoneNumberVerified { get; set; }

        public List<Guid> RoleIds { get; set; } = new List<Guid>();
    }
}
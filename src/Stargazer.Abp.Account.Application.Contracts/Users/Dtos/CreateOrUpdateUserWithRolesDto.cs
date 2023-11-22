using System;
using System.Collections.Generic;

namespace Stargazer.Abp.Account.Application.Contracts.Users.Dtos
{
    public class CreateOrUpdateUserWithRolesDto : CreateUserDto
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
        
        /// <summary>
        /// 允许登录时间开始
        /// </summary>
        public DateTime AllowStartTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 允许登录时间结束
        /// </summary>
        public DateTime AllowEndTime { get; set; } = DateTime.Now.AddYears(100);
        

        /// <summary>
        /// 暂停用户开始日期
        /// </summary>
        public DateTime LockStartTime { get; set; } = DateTime.Now.AddYears(100);

        /// <summary>
        /// 暂停用户结束日期
        /// </summary>
        public DateTime LockEndDate { get; set; } = DateTime.Now.AddYears(100);

        public List<Guid> RoleIds { get; set; } = new List<Guid>();
    }
}
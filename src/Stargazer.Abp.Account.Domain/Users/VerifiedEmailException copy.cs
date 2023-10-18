using System;
using Volo.Abp;

namespace Stargazer.Abp.Account.Domain.Users
{
    public class VerifiedPasswordException : UserFriendlyException
    {
        public VerifiedPasswordException(Guid userId, string password)
            : base(message: "密码必须包含大小写字母和数字，且长度8位以上。")
        {

        }
    }
}
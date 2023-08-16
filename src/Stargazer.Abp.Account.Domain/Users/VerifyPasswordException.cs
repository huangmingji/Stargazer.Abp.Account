using System;
using Volo.Abp;

namespace Stargazer.Abp.Account.Domain.Users
{
    public class VerifyPasswordException : UserFriendlyException
    {
        public VerifyPasswordException(Guid userId, string password)
            : base(message: "账户密码错误", details: $"User ({userId}) password ({password}) error.")
        {

        }
    }
}
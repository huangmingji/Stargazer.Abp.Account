using System;
using Volo.Abp;

namespace Stargazer.Abp.Account.Domain.Users
{
    public class VerifyPasswordException : BusinessException
    {
        public VerifyPasswordException(Guid userId, string password)
            : base(message: $"User ({userId}) password ({password}) error.")
        {

        }
    }
}
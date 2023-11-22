using System;
using Volo.Abp;

namespace Stargazer.Abp.Account.Domain.Shared.Users
{
    public class VerifiedEmailException : UserFriendlyException
    {
        public VerifiedEmailException(Guid userId, string email)
            : base(message: "电子邮件地址验证失败！", details: $"The email {email} does not belong to the user {userId}")
        {

        }
    }
}
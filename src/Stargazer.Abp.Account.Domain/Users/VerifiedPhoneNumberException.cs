using System;
using Volo.Abp;

namespace Stargazer.Abp.Account.Domain.Users
{
    public class VerifiedPhoneNumberException : UserFriendlyException
    {
        public VerifiedPhoneNumberException(Guid userId, string phoneNumber)
            : base(message: "电话号码验证失败！", details: $"The phone number {phoneNumber} does not belong to the user {userId}")
        {

        }
    }
}
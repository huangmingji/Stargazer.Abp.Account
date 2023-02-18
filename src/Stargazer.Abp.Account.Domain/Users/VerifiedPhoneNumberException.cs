using System;
using Volo.Abp;

namespace Stargazer.Abp.Account.Domain.Users
{
    public class VerifiedPhoneNumberException : BusinessException
    {
        public VerifiedPhoneNumberException(Guid userId, string phoneNumber)
            : base(message: $"The phone number {phoneNumber} does not belong to the user {userId}")
        {

        }
    }
}
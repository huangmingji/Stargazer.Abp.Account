using System;
using Volo.Abp;

namespace Stargazer.Abp.Account.Domain.Users
{
    public class VerifiedEmailException : BusinessException
    {
        public VerifiedEmailException(Guid userId, string email)
            : base(message: $"The email {email} does not belong to the user {userId}")
        {

        }
    }
}
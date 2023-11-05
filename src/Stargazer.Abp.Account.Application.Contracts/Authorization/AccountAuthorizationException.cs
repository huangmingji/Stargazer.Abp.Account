using System;
using Volo.Abp;

namespace Stargazer.Abp.Account.Application.Contracts.Authorization;

public class AccountAuthorizationException : BusinessException
{
    public AccountAuthorizationException(Guid? userId, string policyName) : base(
        message: $"User {userId} has no permissions {policyName}.")
    {
    }
}
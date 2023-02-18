using System;
using System.Threading.Tasks;

namespace Stargazer.Abp.Account.Application.Contracts.Authorization;

public interface IAccountAuthorization
{
    Task CheckAccountPolicyAsync(Guid userId, string policyName);
}
using System;
using System.Threading.Tasks;

namespace Stargazer.Abp.Account.Application.Contracts.Authorization;

public interface IAccountAuthorization
{
    Task CheckAccountPolicyAsync(Guid userId, string policyName);
    
    Task CheckCurrentAccountPolicyAsync(string policyName);

    Task<bool> HasAccountPolicyAsync(Guid userId, string policyName);

    Task<bool> HasCurrentAccountPolicyAsync(string policyName);
}
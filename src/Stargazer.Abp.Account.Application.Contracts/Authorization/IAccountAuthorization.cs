namespace Stargazer.Abp.Account.Application.Contracts.Authorization;

public interface IAccountAuthorization
{
    Task CheckAccountPolicyAsync(Guid userId, string policyName);
    
    void CheckCurrentAccountPolicyAsync(string policyName);

    bool HasCurrentAccountPolicyAsync(string policyName);
}
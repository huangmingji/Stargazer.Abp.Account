namespace Stargazer.Abp.Authorization.Application.Contracts.Permissions;

public interface IAccountAuthorization
{    
    void CheckCurrentAccountPolicy(string policyName);

    bool HasCurrentAccountPolicy(string policyName);
}
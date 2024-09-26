using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Stargazer.Abp.Authorization.Application.Contracts.Permissions;

public class AccountAuthorization : IAccountAuthorization, ITransientDependency
{
    private readonly ICurrentUser _currentUser;

    public AccountAuthorization(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public void CheckCurrentAccountPolicy(string policyName)
    {
        var permissions = _currentUser.GetAllClaims().Where(x=> x.Type == "permission").ToList();
        if (permissions.Any(x => x.Value == policyName) == false)
        {
            throw new AccountAuthorizationException(_currentUser.Id, policyName);
        }
    }

    public bool HasCurrentAccountPolicy(string policyName)
    {
        var permissions = _currentUser.GetAllClaims().Where(x=> x.Type == "permission").ToList();
        return permissions.Any(x => x.Value == policyName);
    }
}
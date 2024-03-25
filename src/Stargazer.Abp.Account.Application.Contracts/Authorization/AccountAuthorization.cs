using Stargazer.Abp.Account.Application.Contracts.Users;
using Stargazer.Abp.Account.Application.Contracts.Users.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Stargazer.Abp.Account.Application.Contracts.Authorization;

public class AccountAuthorization : IAccountAuthorization, ITransientDependency
{
    private readonly ICurrentUser _currentUser;

    public AccountAuthorization(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public void CheckAccountPolicy(UserDto user, string policyName)
    {
        if (!user.UserRoles.Exists(role => role.RoleData.Permissions.Exists(data => data.PermissionData.Permission == policyName)))
        {
            throw new AccountAuthorizationException(user.Id, policyName);
        }
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
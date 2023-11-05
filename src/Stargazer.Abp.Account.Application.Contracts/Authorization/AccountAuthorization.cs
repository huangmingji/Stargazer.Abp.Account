using Stargazer.Abp.Account.Application.Contracts.Users;
using Stargazer.Abp.Account.Domain.Repository;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Stargazer.Abp.Account.Application.Contracts.Authorization;

public class AccountAuthorization : IAccountAuthorization, ITransientDependency
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;

    public AccountAuthorization(ICurrentUser currentUser, IUserRepository userRepository)
    {
        _currentUser = currentUser;
        _userRepository = userRepository;
    }

    public async Task CheckAccountPolicyAsync(Guid userId, string policyName)
    {
        var user = await _userRepository.GetAsync(userId);
        if (!user.UserRoles.Exists(role => role.RoleData.Permissions.Exists(data => data.Permission == policyName)))
        {
            throw new AccountAuthorizationException(userId, policyName);
        }
    }

    public void CheckCurrentAccountPolicyAsync(string policyName)
    {
        var permissions = _currentUser.GetAllClaims().Where(x=> x.Type == "permission").ToList();
        if (permissions.Any(x => x.Value == policyName) == false)
        {
            throw new AccountAuthorizationException(_currentUser.Id, policyName);
        }
    }

    public bool HasCurrentAccountPolicyAsync(string policyName)
    {
        var permissions = _currentUser.GetAllClaims().Where(x=> x.Type == "permission").ToList();
        return permissions.Any(x => x.Value == policyName);
    }
}
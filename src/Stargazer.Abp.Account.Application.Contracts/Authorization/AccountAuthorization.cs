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

    public async Task CheckCurrentAccountPolicyAsync(string policyName)
    {
        if (_currentUser.Id == null)
        {
            throw new AccountAuthorizationException(_currentUser.Id, policyName);
        }
        await CheckAccountPolicyAsync((Guid)_currentUser.Id, policyName);
    }

    public async Task<bool> HasAccountPolicyAsync(Guid userId, string policyName)
    {
        var user = await _userRepository.GetAsync(userId);
        return user.UserRoles.Exists(role => role.RoleData.Permissions.Exists(data => data.Permission == policyName));
    }

    public async Task<bool> HasCurrentAccountPolicyAsync(string policyName)
    {
        if (_currentUser.Id == null)
        {
            return false;
        }
        return await HasAccountPolicyAsync((Guid)_currentUser.Id, policyName);
    }
}
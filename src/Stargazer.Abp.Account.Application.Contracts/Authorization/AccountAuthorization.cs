using System;
using System.Threading.Tasks;
using Stargazer.Abp.Account.Domain.Repository;
using Volo.Abp.DependencyInjection;

namespace Stargazer.Abp.Account.Application.Contracts.Authorization;

public class AccountAuthorization : IAccountAuthorization, ITransientDependency
{
    private readonly IUserRepository _userRepository;

    public AccountAuthorization(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task CheckAccountPolicyAsync(Guid userId, string policyName)
    {
        var user = await _userRepository.GetAsync(userId);
        if (!user.UserRoles.Exists(role => role.RoleData.Permissions.Exists(data => data.Permission == policyName )))
        {
            throw new AccountAuthorizationException(userId, policyName);
        }
    }
}
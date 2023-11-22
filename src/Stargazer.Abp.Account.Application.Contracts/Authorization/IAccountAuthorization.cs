using Stargazer.Abp.Account.Application.Contracts.Users.Dtos;

namespace Stargazer.Abp.Account.Application.Contracts.Authorization;

public interface IAccountAuthorization
{
    void CheckAccountPolicy(UserDto user, string policyName);
    
    void CheckCurrentAccountPolicy(string policyName);

    bool HasCurrentAccountPolicy(string policyName);
}
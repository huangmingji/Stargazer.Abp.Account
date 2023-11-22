using Volo.Abp;

namespace Stargazer.Abp.Account.Domain.Users;

public class UserLockLoginException: UserFriendlyException
{
    public UserLockLoginException(Guid userId) :
        base("User is locked", "UserLocked",  $"User({userId}) is locked")
    {
    }
}
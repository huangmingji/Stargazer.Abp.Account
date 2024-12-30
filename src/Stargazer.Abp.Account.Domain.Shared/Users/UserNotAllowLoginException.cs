using Volo.Abp;

namespace Stargazer.Abp.Account.Domain.Shared.Users;

public class UserNotAllowLoginException: UserFriendlyException
{
    public UserNotAllowLoginException(Guid userId) :
        base("User is not allow login", "UserNotAllowLogin",  $"User({userId}) is not allow login")
    {
    }
}
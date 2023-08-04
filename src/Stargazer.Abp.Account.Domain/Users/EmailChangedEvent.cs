namespace Stargazer.Abp.Account.Domain.Users;
public class EmailChangedEvent
{
    public EmailChangedEvent(UserData user, string email)
    {
        User = user;
        Email = email;
    }

    public UserData User { get; set; }

    public string Email { get; set; }

}
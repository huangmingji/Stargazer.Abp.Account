namespace Stargazer.Abp.Account.Domain.Users;
public class EmailChangedEvent
{
    public EmailChangedEvent(string email)
    {
        Email = email;
    }

    public string Email { get; set; }

}
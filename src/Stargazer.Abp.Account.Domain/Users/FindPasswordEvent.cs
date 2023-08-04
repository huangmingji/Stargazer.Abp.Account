namespace Stargazer.Abp.Account.Domain.Users;

public class FindPasswordEvent {
    
    public FindPasswordEvent(UserData user, string email)
    {
        User = user;
        Email = email;
    }

    public UserData User { get; set; }

    public string Email { get; set; }
    
}
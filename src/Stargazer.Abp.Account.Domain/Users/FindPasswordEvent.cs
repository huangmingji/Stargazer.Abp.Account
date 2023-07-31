namespace Stargazer.Abp.Account.Domain.Users;

public class FindPasswordEvent {
    
    public FindPasswordEvent(string email)
    {
        Email = email;
    }

    public string Email { get; set; }
    
}
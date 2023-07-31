namespace Stargazer.Abp.Account.Application.Contracts.Users.Dtos;

public class ResetPasswordDto
{
    public string Email { get; set; } = "";
    
    public string Token { get; set; } = "";
    
    public string Password { get; set; } = "";
}
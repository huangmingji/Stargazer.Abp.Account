namespace Stargazer.Abp.Account.Application.Contracts.Users.Dtos;

public class VerifyEmailDto
{
    public string Email { get; set; } = "";

    public string Token { get; set; } = "";
}
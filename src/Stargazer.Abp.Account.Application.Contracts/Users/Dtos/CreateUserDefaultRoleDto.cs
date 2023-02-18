namespace Stargazer.Abp.Account.Application.Contracts.Users.Dtos
{
    public class CreateUserDefaultRoleDto
    {
        public string UserName { get; set; }
        
        public string Account { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string HeadIcon { get; set;}
        
        public string Password { get; set; }
    }
}
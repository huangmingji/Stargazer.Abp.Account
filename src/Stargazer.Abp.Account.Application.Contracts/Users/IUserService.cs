using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stargazer.Abp.Account.Application.Contracts.Users.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Stargazer.Abp.Account.Application.Contracts.Users
{
    public interface IUserService : IApplicationService
    {
        Task<UserDto> CreateAsync(CreateUserDto input);
        
        Task<UserDto> CreateAsync(CreateUserWithRolesDto input);

        Task<UserDto> CreateAsync(UpdateUserDto input);

        Task<UserDto> CreateAsync(CreateUserDefaultRoleDto input);
        
        Task<UserDto> GetAsync(Guid id);
        
        Task<PagedResultDto<UserDto>> GetListAsync(int pageIndex,
            int pageSize, string? name = null, string? account = null,
            string? email = null, string? phoneNumber = null);

        Task<List<UserDto>> GetListAsync(List<Guid> userIds);

        Task DeleteAsync(Guid id);

        Task<UserDto?> FindByPhoneNumberAsync(string phoneNumber);

        Task<UserDto?> FindByEmailAsync(string email);

        Task<UserDto?> FindByAccountAsync(string account);

        Task<UserDto> UpdateUserNameAsync(Guid id, UpdateUserNameDto input);

        Task<UserDto> UpdatePhoneNumberAsync(Guid id, string phoneNumber);
        
        Task<UserDto> UpdateEmailAsync(Guid id, string email);
        
        Task<UserDto> UpdateAvatarAsync(Guid id, string avatar);

        Task<UserDto> UpdatePasswordAsync(Guid id, UpdateUserPasswordDto input);
        
        Task<UserDto> UpdatePasswordAsync(Guid id, UpdatePasswordDto input);

        Task<UserDto> VerifyPasswordAsync(VerifyPasswordDto input);
        
        Task<UserDto> VerifyPasswordAsync(Guid id, string password);

        Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto input);

        Task<UserDto> UpdateUserRoleAsync(Guid id, UpdateUserRoleDto input);

        Task<UserDto> VerifiedPhoneNumberAsync(Guid id, string phoneNumber);

        Task<UserDto> VerifiedEmailAsync(Guid id, string email);

        Task<UserDto> UpdateAccountAsync(Guid id, UpdateAccountDto input);

        Task ResetPasswordAsync(ResetPasswordDto input);

        Task FindPasswordAsync(FindPasswordDto input);
    }
}
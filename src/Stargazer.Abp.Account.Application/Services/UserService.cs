using Stargazer.Abp.Account.Application.Contracts.Authorization;
using Stargazer.Abp.Account.Application.Contracts.Users;
using Stargazer.Abp.Account.Application.Contracts.Users.Dtos;
using Stargazer.Abp.Account.Domain.Repository;
using Stargazer.Abp.Account.Domain.Users;
using Lemon.Common.Extend;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using System.Linq.Dynamic.Core;
using Stargazer.Abp.Authorization.Application.Contracts.Permissions;

namespace Stargazer.Abp.Account.Application.Services;

public class UserService : ApplicationService, IUserService
{
    private EmailService EmailService => this.LazyServiceProvider.LazyGetRequiredService<EmailService>();
    private IConfiguration Configuration => this.LazyServiceProvider.LazyGetRequiredService<IConfiguration>();
    private IUserRepository UserRepository => this.LazyServiceProvider.LazyGetRequiredService<IUserRepository>();
    private IAccountAuthorization AccountAuthorization => this.LazyServiceProvider.LazyGetRequiredService<IAccountAuthorization>();
    private IRoleRepository RoleRepository => this.LazyServiceProvider.LazyGetRequiredService<IRoleRepository>();
    private IDistributedCache<string> Cache => this.LazyServiceProvider.LazyGetRequiredService<IDistributedCache<string>>();
    private ILogger<UserService> UserServiceLogger => this.LazyServiceProvider.LazyGetRequiredService<ILogger<UserService>>();

    public async Task<UserDto> CreateAsync(CreateUserDto input)
    {
        if (await UserRepository.AnyAsync(x => x.Email == input.Email))
        {
            throw new UserFriendlyException("电子邮箱地址已注册");
        }

        var userData = new UserData(GuidGenerator.Create(), input.UserName);
        userData.SetPassword(input.Password);
        bool verifyEmail = Configuration.GetSection("App:VerifyEmail").Value?.ToBool() ?? false;
        userData.SetEmail(input.Email, !verifyEmail);
        await UserRepository.InsertAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> CreateAsync(CreateOrUpdateUserWithRolesDto input)
    {
        var userData = new UserData(id: GuidGenerator.Create(),
            account: input.Account,
            name: input.UserName,
            phoneNumber: input.PhoneNumber);

        if (!string.IsNullOrWhiteSpace(input.Password))
        {
            userData.SetPassword(input.Password);
        }

        userData.SetEmail(input.Email, input.EmailVerified);
        userData.SetPhoneNumber(input.PhoneNumber, input.PhoneNumberVerified);

        userData.AllowUser(input.AllowStartTime, input.AllowEndTime);
        userData.LockUser(input.LockStartTime, input.LockEndDate);

        input.RoleIds.ForEach(roleId => { userData.AddRole(GuidGenerator.Create(), roleId); });
        await UserRepository.InsertAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> CreateAsync(UpdateUserDto input)
    {
        if (await UserRepository.AnyAsync(x => x.Account == input.Account))
        {
            throw new UserFriendlyException("账号已注册");
        }

        if (!string.IsNullOrWhiteSpace(input.Email) && await UserRepository.AnyAsync(x => x.Email == input.Email))
        {
            throw new UserFriendlyException("电子邮箱地址已注册");
        }

        if (!string.IsNullOrWhiteSpace(input.PhoneNumber) &&
            await UserRepository.AnyAsync(x => x.PhoneNumber == input.PhoneNumber))
        {
            throw new UserFriendlyException("手机号已注册");
        }

        if (input.RoleIds == null || input.RoleIds.Count == 0)
        {
            throw new UserFriendlyException("请选择用户角色");
        }

        var userData = new UserData(GuidGenerator.Create(), input.Account, input.Name, input.PhoneNumber);
        userData.SetPassword(input.Password);

        bool verifyEmail = Configuration.GetSection("App:VerifyEmail").Value?.ToBool() ?? false;
        userData.SetEmail(input.Email, !verifyEmail);

        Dictionary<Guid, Guid> roles = new Dictionary<Guid, Guid>();
        input.RoleIds.ForEach(item => { roles.Add(GuidGenerator.Create(), item); });
        userData.SetRoles(roles);

        var result = await UserRepository.InsertAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> CreateAsync(CreateUserDefaultRoleDto input)
    {
        if (await UserRepository.AnyAsync(x => x.Account == input.Account))
        {
            throw new UserFriendlyException("账号已注册");
        }

        if (!string.IsNullOrWhiteSpace(input.Email) && await UserRepository.AnyAsync(x => x.Email == input.Email))
        {
            throw new UserFriendlyException("电子邮箱地址已注册");
        }

        if (!string.IsNullOrWhiteSpace(input.PhoneNumber) &&
            await UserRepository.AnyAsync(x => x.PhoneNumber == input.PhoneNumber))
        {
            throw new UserFriendlyException("手机号已注册");
        }

        var role = await RoleRepository.GetAsync(x => x.IsDefault);
        var userData = new UserData(GuidGenerator.Create(), input.Account, input.UserName, input.PhoneNumber);
        userData.SetPassword(input.Password);

        bool verifyEmail = Configuration.GetSection("App:VerifyEmail").Value?.ToBool() ?? false;
        userData.SetEmail(input.Email, !verifyEmail);

        userData.AddRole(GuidGenerator.Create(), role.Id);
        var result = await UserRepository.InsertAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> GetAsync(Guid id)
    {
        var userData = await UserRepository.GetAsync(id);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<PagedResultDto<UserDto>> GetListAsync(int pageIndex, int pageSize, string? searchText = null)
    {
        var expression = Expressionable.Create<UserData>()
            .AndIf(!string.IsNullOrWhiteSpace(searchText), x => x.NickName.Contains(searchText) || x.Account.Contains(searchText) || x.Email.Contains(searchText) || x.PhoneNumber.Contains(searchText));
        int total = await UserRepository.CountAsync(expression);
        var data = await UserRepository.GetListByPageAsync(expression, pageIndex, pageSize);
        return new PagedResultDto<UserDto>()
        {
            TotalCount = total,
            Items = ObjectMapper.Map<List<UserData>, List<UserDto>>(data)
        };
    }

    public async Task<List<UserDto>> GetListAsync(List<Guid> userIds)
    {
        return ObjectMapper.Map<List<UserData>, List<UserDto>>(
            await UserRepository.GetListAsync(x => userIds.Contains(x.Id)));
    }

    public async Task DeleteAsync(Guid id)
    {
        await UserRepository.DeleteAsync(x => x.Id == id);
    }

    public async Task<UserDto?> FindByPhoneNumberAsync(string phoneNumber)
    {
        var userData = await UserRepository.FindAsync(x => x.PhoneNumber == phoneNumber);
        if (userData == null) return null;
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto?> FindByEmailAsync(string email)
    {
        var userData = await UserRepository.FindAsync(x => x.Email == email);
        if (userData == null) return null;
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto?> FindByAccountAsync(string account)
    {
        var userData = await UserRepository.FindAsync(x => x.Account == account);
        if (userData == null) return null;
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> UpdatePhoneNumberAsync(Guid id, string phoneNumber)
    {
        bool verifyPhoneNumber = Configuration.GetSection("App:VerifyPhoneNumber").Value?.ToBool() ?? false;
        var userData = await UserRepository.GetAsync(x => x.Id == id);
        userData.SetPhoneNumber(phoneNumber, !verifyPhoneNumber);
        var result = await UserRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task<UserDto> UpdateEmailAsync(Guid id, string email)
    {
        bool verifyEmail = Configuration.GetSection("App:VerifyEmail").Value?.ToBool() ?? false;

        if (await UserRepository.AnyAsync(x => x.Email == email && x.Id != id))
        {
            throw new UserFriendlyException("电子邮箱地址已注册");
        }

        var userData = await UserRepository.GetAsync(x => x.Id == id);
        userData.SetEmail(email, !verifyEmail);
        var result = await UserRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task<UserDto> UpdateAvatarAsync(Guid id, string avatar)
    {
        var userData = await UserRepository.GetAsync(x => x.Id == id);
        userData.SetHeadIcon(avatar);
        var result = await UserRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task<UserDto> UpdatePasswordAsync(Guid id, UpdateUserPasswordDto input)
    {
        var userData = await UserRepository.GetAsync(x => x.Id == id);
        userData.VerifyPassword(input.OldPassword);
        userData.SetPassword(input.Password);
        var result = await UserRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task<UserDto> UpdatePasswordAsync(Guid id, UpdatePasswordDto input)
    {
        var userData = await UserRepository.GetAsync(x => x.Id == id);
        userData.SetPassword(input.Password);
        var result = await UserRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task<UserDto> VerifyPasswordAsync(VerifyPasswordDto input)
    {
        UserData? userData = await UserRepository.FindAsync(x =>
            x.Account == input.Name
            || x.PhoneNumber == input.Name
            || x.Email == input.Name);
        if (userData == null)
        {
            throw new UserFriendlyException("账户密码错误");
        }

        if (input.Name == userData.Email && !userData.EmailVerified)
        {
            await EmailService.EmailChanged(new EmailChangedEvent(userData, input.Name));
            throw new UserFriendlyException("电子邮箱地址未通过验证，请查看邮箱进行验证");
        }

        userData.VerifyPassword(input.Password);
        userData.CheckAllowTime();
        userData.CheckLockTime();
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> VerifyPasswordAsync(Guid id, string password)
    {
        UserData? userData = await UserRepository.FindAsync(x => x.Id == id);
        if (userData == null)
        {
            throw new UserFriendlyException("账号不存在");
        }

        userData.VerifyPassword(password);
        userData.CheckAllowTime();
        userData.CheckLockTime();
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto input)
    {
        UserData userData = await UserRepository.GetAsync(x => x.Id == id);

        if (await UserRepository.AnyAsync(x => x.Id != id && x.Account == input.Account))
        {
            throw new UserFriendlyException("该账号已注册");
        }

        if (!string.IsNullOrWhiteSpace(input.Email) &&
            await UserRepository.AnyAsync(x => x.Id != id && x.Email == input.Email))
        {
            throw new UserFriendlyException("电子邮件地址已注册");
        }

        if (!string.IsNullOrWhiteSpace(input.PhoneNumber) &&
            await UserRepository.AnyAsync(x => x.Id != id && x.PhoneNumber == input.PhoneNumber))
        {
            throw new UserFriendlyException("手机号码已注册");
        }

        if (input.RoleIds != null)
        {
            AccountAuthorization.CheckCurrentAccountPolicy(AccountPermissions.User.Update);
            if (input.RoleIds is { Count: 0 })
            {
                throw new UserFriendlyException("请选择用户角色");
            }

            Dictionary<Guid, Guid> roles = new Dictionary<Guid, Guid>();
            input.RoleIds.ForEach(item => { roles.Add(GuidGenerator.Create(), item); });
            userData.SetRoles(roles);
        }

        if (!string.IsNullOrWhiteSpace(input.Password))
        {
            userData.SetPassword(input.Password);
        }

        userData.SetData(input.Name, input.Account, input.Email, input.PhoneNumber);
        var result = await UserRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> UpdatePersonalSettingsAsync(Guid id, UpdatePersonalSettingsDto input)
    {
        UserData userData = await UserRepository.GetAsync(x => x.Id == id);

        if(await UserRepository.AnyAsync(x => x.Id != id && x.NickName == input.Name))
        {
            throw new UserFriendlyException("昵称已存在");
        }

        if (!string.IsNullOrWhiteSpace(input.Email) &&
            await UserRepository.AnyAsync(x => x.Id != id && x.Email == input.Email))
        {
            throw new UserFriendlyException("电子邮件地址已存在");
        }

        userData.SetName(input.Name);
        bool verifyEmail = Configuration.GetSection("App:VerifyEmail").Value?.ToBool() ?? false;
        userData.SetEmail(input.Email, !verifyEmail);
        userData.SetPersonalProfile(input.PersonalProfile);
        userData.SetAddress(input.Country, input.Province, input.City, input.District, input.Address);
        userData.SetTelephoneNumber(input.TelephoneNumberAreaCode, input.TelephoneNumber);
        var result = await UserRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> UpdateUserRoleAsync(Guid id, UpdateUserRoleDto input)
    {
        UserData userData = await UserRepository.GetAsync(x => x.Id == id);
        Dictionary<Guid, Guid> roles = new Dictionary<Guid, Guid>();
        input.RoleIds.ForEach(item => { roles.Add(GuidGenerator.Create(), item); });
        userData.SetRoles(roles);
        var result = await UserRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> VerifiedPhoneNumberAsync(Guid id, string phoneNumber)
    {
        UserData userData = await UserRepository.GetAsync(x => x.Id == id);
        userData.VerifiedPhoneNumber(phoneNumber);
        var result = await UserRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task<UserDto> VerifiedEmailAsync(Guid id, string email)
    {
        UserData userData = await UserRepository.GetAsync(x => x.Id == id);
        userData.VerifiedEmail(email);
        var result = await UserRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task<UserDto> UpdateUserNameAsync(Guid id, UpdateUserNameDto input)
    {
        UserData userData = await UserRepository.GetAsync(x => x.Id == id);
        userData.SetName(input.Name);
        var result = await UserRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task<UserDto> UpdateAccountAsync(Guid id, UpdateAccountDto input)
    {
        UserData userData = await UserRepository.GetAsync(x => x.Id == id);
        userData.SetAccount(input.Account);
        var result = await UserRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task ResetPasswordAsync(ResetPasswordDto input)
    {
        var user = await UserRepository.FindAsync(x => x.Email == input.Email);
        if (user == null)
        {
            UserServiceLogger.LogError($"###ResetPasswordAsync###------{input.Email} not found.");
            throw new UserFriendlyException("token已过期。");
        }

        var token = await Cache.GetAsync($"FindPasswordToken:{input.Email}");
        if (token != input.Token)
        {
            UserServiceLogger.LogError($"###ResetPasswordAsync###------{input.Email} verify token error.");
            throw new UserFriendlyException("token已过期。");
        }
        await Cache.RemoveAsync($"FindPasswordToken:{input.Email}");
        user.SetPassword(input.Password);
        await UserRepository.UpdateAsync(user);
    }

    public async Task FindPasswordAsync(FindPasswordDto input)
    {
        var user = await UserRepository.FindAsync(x => x.Email == input.Email);
        if (user == null) return;
        await EmailService.FindPassword(new FindPasswordEvent(user, input.Email));
    }

    public async Task<UserDto> UpdateUserAsync(Guid id, CreateOrUpdateUserWithRolesDto input)
    {
        var userData = await UserRepository.GetAsync(id);
        userData.SetAccount(input.Account);
        userData.SetName(input.UserName);
        userData.SetEmail(input.Email, input.EmailVerified);
        userData.SetPhoneNumber(input.PhoneNumber, input.PhoneNumberVerified);
        userData.AllowUser(input.AllowStartTime, input.AllowEndTime);
        userData.LockUser(input.LockStartTime, input.LockEndDate);
        if (!string.IsNullOrWhiteSpace(input.Password))
        {
            userData.SetPassword(input.Password);
        }

        Dictionary<Guid, Guid> roleIds = new Dictionary<Guid, Guid>();
        input.RoleIds.ForEach(roleId => { roleIds.Add(GuidGenerator.Create(), roleId); });
        userData.SetRoles(roleIds);

        await UserRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> GetIncludingDeletedAsync(Guid id)
    {
        var userData = await UserRepository.GetAsync(x=> x.Id == id && (x.IsDeleted || !x.IsDeleted));
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<PagedResultDto<UserDto>> GetListIncludingDeletedAsync(int pageIndex, int pageSize, string? searchText = null)
    {
        var queryable = (await UserRepository.GetQueryableAsync())
            .Where(x=> x.IsDeleted || !x.IsDeleted)
            .WhereIf(!string.IsNullOrWhiteSpace(searchText), x => x.NickName.Contains(searchText) || x.Account.Contains(searchText) || x.Email.Contains(searchText) || x.PhoneNumber.Contains(searchText));
        int total = queryable.Count();
        var data = queryable.Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();
        return new PagedResultDto<UserDto>()
        {
            TotalCount = total,
            Items = ObjectMapper.Map<List<UserData>, List<UserDto>>(data)
        };
    }

    public async Task DeleteIncludingDeletedAsync(Guid id)
    {
        await UserRepository.DeleteAsync(x=> x.Id == id && (x.IsDeleted || !x.IsDeleted));
    }

    public async Task<UserDto> UpdateUserIncludingDeletedAsync(Guid id, CreateOrUpdateUserWithRolesDto input)
    {
        var userData = await UserRepository.GetAsync(x=> x.Id == id && (x.IsDeleted ||!x.IsDeleted));
        userData.SetAccount(input.Account);
        userData.SetName(input.UserName);
        userData.SetEmail(input.Email, input.EmailVerified);
        userData.SetPhoneNumber(input.PhoneNumber, input.PhoneNumberVerified);
        userData.AllowUser(input.AllowStartTime, input.AllowEndTime);
        userData.LockUser(input.LockStartTime, input.LockEndDate);
        if (!string.IsNullOrWhiteSpace(input.Password))
        {
            userData.SetPassword(input.Password);
        }

        Dictionary<Guid, Guid> roleIds = new Dictionary<Guid, Guid>();
        input.RoleIds.ForEach(roleId => { roleIds.Add(GuidGenerator.Create(), roleId); });
        userData.SetRoles(roleIds);

        await UserRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }
}
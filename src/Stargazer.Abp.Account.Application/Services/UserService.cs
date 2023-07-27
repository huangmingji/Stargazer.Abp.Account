﻿using Stargazer.Abp.Account.Application.Contracts.Authorization;
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
using Volo.Abp.EventBus.Local;

namespace Stargazer.Abp.Account.Application.Services;

public class UserService : ApplicationService, IUserService
{
    private readonly ILocalEventBus _localEventBus;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IAccountAuthorization _accountAuthorization;
    private readonly IRoleRepository _roleRepository;
    private ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository,
        IAccountAuthorization accountAuthorization,
        IRoleRepository roleRepository,
        IConfiguration configuration,
        ILogger<UserService> logger, ILocalEventBus localEventBus)
    {
        _userRepository = userRepository;
        _accountAuthorization = accountAuthorization;
        _roleRepository = roleRepository;
        _configuration = configuration;
        _logger = logger;
        _localEventBus = localEventBus;
    }

    public async Task<UserDto> CreateAsync(CreateUserDto input)
    {
        if (await _userRepository.AnyAsync(x => x.Email == input.Email))
        {
            throw new UserFriendlyException("电子邮箱地址已注册");
        }

        var userData = new UserData(GuidGenerator.Create(), input.Password, input.UserName);
        bool verifyEmail = _configuration.GetSection("App:VerifyEmail").Value?.ToBool() ?? false;
        userData.SetEmail(input.Email, !verifyEmail);
        var role = await _roleRepository.FirstOrDefaultAsync(x => x.IsDefault);
        if (role != null)
        {
            userData.AddRole(GuidGenerator.Create(), role.Id);
        }

        await _userRepository.InsertAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> CreateAsync(CreateUserWithRolesDto input)
    {
        var userData = new UserData(id: GuidGenerator.Create(),
            account: input.Account,
            password: input.Password,
            name: input.UserName,
            phoneNumber: input.PhoneNumber);
        bool verifyEmail = _configuration.GetSection("App:VerifyEmail").Value?.ToBool() ?? false;
        userData.SetEmail(input.Email, !verifyEmail);
        input.RoleIds.ForEach(roleId => { userData.AddRole(GuidGenerator.Create(), roleId); });
        await _userRepository.InsertAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> CreateAsync(UpdateUserDto input)
    {
        if (await _userRepository.AnyAsync(x => x.Account == input.Account))
        {
            throw new UserFriendlyException("账号已注册");
        }

        if (!string.IsNullOrWhiteSpace(input.Email) && await _userRepository.AnyAsync(x => x.Email == input.Email))
        {
            throw new UserFriendlyException("电子邮箱地址已注册");
        }

        if (!string.IsNullOrWhiteSpace(input.PhoneNumber) &&
            await _userRepository.AnyAsync(x => x.PhoneNumber == input.PhoneNumber))
        {
            throw new UserFriendlyException("手机号已注册");
        }

        if (input.RoleIds == null || input.RoleIds.Count == 0)
        {
            throw new UserFriendlyException("请选择用户角色");
        }

        var userData = new UserData(GuidGenerator.Create(), input.Account, input.Password, input.Name,
            input.PhoneNumber);
        bool verifyEmail = _configuration.GetSection("App:VerifyEmail").Value?.ToBool() ?? false;
        userData.SetEmail(input.Email, !verifyEmail);
        Dictionary<Guid, Guid> roles = new Dictionary<Guid, Guid>();
        input.RoleIds.ForEach(item => { roles.Add(GuidGenerator.Create(), item); });
        userData.SetRoles(roles);
        var result = await _userRepository.InsertAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> CreateAsync(CreateUserDefaultRoleDto input)
    {
        if (await _userRepository.AnyAsync(x => x.Account == input.Account))
        {
            throw new UserFriendlyException("账号已注册");
        }

        if (!string.IsNullOrWhiteSpace(input.Email) && await _userRepository.AnyAsync(x => x.Email == input.Email))
        {
            throw new UserFriendlyException("电子邮箱地址已注册");
        }

        if (!string.IsNullOrWhiteSpace(input.PhoneNumber) &&
            await _userRepository.AnyAsync(x => x.PhoneNumber == input.PhoneNumber))
        {
            throw new UserFriendlyException("手机号已注册");
        }

        var role = await _roleRepository.GetAsync(x => x.IsDefault);
        var userData = new UserData(GuidGenerator.Create(), input.Account, input.Password, input.UserName,
            input.PhoneNumber);
        bool verifyEmail = _configuration.GetSection("App:VerifyEmail").Value?.ToBool() ?? false;
        userData.SetEmail(input.Email, !verifyEmail);
        userData.AddRole(GuidGenerator.Create(), role.Id);
        var result = await _userRepository.InsertAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> GetAsync(Guid id)
    {
        var userData = await _userRepository.GetAsync(id);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<PagedResultDto<UserDto>> GetListAsync(int pageIndex, int pageSize, string? name = null,
        string? account = null, string? email = null, string? phoneNumber = null)
    {
        var expression = Expressionable.Create<UserData>()
            .AndIf(!string.IsNullOrWhiteSpace(name), x => x.NickName == name)
            .AndIf(!string.IsNullOrWhiteSpace(account), x => x.Account == account)
            .AndIf(!string.IsNullOrWhiteSpace(email), x => x.Email == email)
            .AndIf(!string.IsNullOrWhiteSpace(phoneNumber), x => x.PhoneNumber == phoneNumber);
        int total = await _userRepository.CountAsync(expression);
        var data = await _userRepository.GetListByPageAsync(expression, pageIndex, pageSize);
        return new PagedResultDto<UserDto>()
        {
            TotalCount = total,
            Items = ObjectMapper.Map<List<UserData>, List<UserDto>>(data)
        };
    }

    public async Task<List<UserDto>> GetListAsync(List<Guid> userIds)
    {
        return ObjectMapper.Map<List<UserData>, List<UserDto>>(
            await _userRepository.GetListAsync(x => userIds.Contains(x.Id)));
    }

    public async Task DeleteAsync(Guid id)
    {
        await _userRepository.DeleteAsync(x => x.Id == id);
    }

    public async Task<UserDto> FindByPhoneNumberAsync(string phoneNumber)
    {
        var userData = await _userRepository.FindAsync(x => x.PhoneNumber == phoneNumber);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> FindByEmailAsync(string email)
    {
        var userData = await _userRepository.FindAsync(x => x.Email == email);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> FindByAccountAsync(string account)
    {
        var userData = await _userRepository.FindAsync(x => x.Account == account);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> UpdatePhoneNumberAsync(Guid id, string phoneNumber)
    {
        bool verifyPhoneNumber = _configuration.GetSection("App:VerifyPhoneNumber").Value?.ToBool() ?? false;
        var userData = await _userRepository.GetAsync(x => x.Id == id);
        userData.SetPhoneNumber(phoneNumber, !verifyPhoneNumber);
        var result = await _userRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task<UserDto> UpdateEmailAsync(Guid id, string email)
    {
        bool verifyEmail = _configuration.GetSection("App:VerifyEmail").Value?.ToBool() ?? false;

        if (await _userRepository.AnyAsync(x => x.Email == email && x.Id != id))
        {
            throw new UserFriendlyException("电子邮箱地址已注册");
        }

        var userData = await _userRepository.GetAsync(x => x.Id == id);
        userData.SetEmail(email, !verifyEmail);
        var result = await _userRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task<UserDto> UpdateAvatarAsync(Guid id, string avatar)
    {
        var userData = await _userRepository.GetAsync(x => x.Id == id);
        userData.SetHeadIcon(avatar);
        var result = await _userRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task<UserDto> UpdatePasswordAsync(Guid id, UpdateUserPasswordDto input)
    {
        var userData = await _userRepository.GetAsync(x => x.Id == id);
        userData.VerifyPassword(input.OldPassword);
        userData.SetPassword(input.Password);
        var result = await _userRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task<UserDto> UpdatePasswordAsync(Guid id, UpdatePasswordDto input)
    {
        var userData = await _userRepository.GetAsync(x => x.Id == id);
        userData.SetPassword(input.Password);
        var result = await _userRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task<UserDto> VerifyPasswordAsync(VerifyPasswordDto input)
    {
        UserData userData = await _userRepository.FindAsync(x =>
            x.Account == input.Name
            || x.PhoneNumber == input.Name
            || x.Email == input.Name);
        if (userData == null)
        {
            throw new UserFriendlyException("账号密码错误");
        }

        if (input.Name == userData.Email && !userData.EmailVerified)
        {
            await _localEventBus.PublishAsync(new EmailChangedEvent(userData, userData.Email));
            throw new UserFriendlyException("电子邮箱地址未通过验证，请查看邮箱进行验证");
        }

        userData.VerifyPassword(input.Password);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> VerifyPasswordAsync(Guid id, string password)
    {
        UserData userData = await _userRepository.FindAsync(x => x.Id == id);
        if (userData == null)
        {
            throw new UserFriendlyException("账号不存在");
        }

        userData.VerifyPassword(password);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> UpdateUserAsync(Guid id, UpdateUserDto input)
    {
        UserData userData = await _userRepository.GetAsync(x => x.Id == id);

        if (await _userRepository.AnyAsync(x => x.Id != id && x.Account == input.Account))
        {
            throw new UserFriendlyException("该账号已注册");
        }

        if (!string.IsNullOrWhiteSpace(input.Email) &&
            await _userRepository.AnyAsync(x => x.Id != id && x.Email == input.Email))
        {
            throw new UserFriendlyException("电子邮件地址已注册");
        }

        if (!string.IsNullOrWhiteSpace(input.PhoneNumber) &&
            await _userRepository.AnyAsync(x => x.Id != id && x.PhoneNumber == input.PhoneNumber))
        {
            throw new UserFriendlyException("手机号码已注册");
        }

        if (input.RoleIds != null)
        {
            await _accountAuthorization.CheckAccountPolicyAsync(CurrentUser.Id.GetValueOrDefault(),
                AccountPermissions.User.Update);
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

        userData.Set(input.Name, input.Account, input.Email, input.PhoneNumber);
        var result = await _userRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> UpdateUserRoleAsync(Guid id, UpdateUserRoleDto input)
    {
        UserData userData = await _userRepository.GetAsync(x => x.Id == id);
        Dictionary<Guid, Guid> roles = new Dictionary<Guid, Guid>();
        input.RoleIds.ForEach(item => { roles.Add(GuidGenerator.Create(), item); });
        userData.SetRoles(roles);
        var result = await _userRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(userData);
    }

    public async Task<UserDto> VerifiedPhoneNumberAsync(Guid id, string phoneNumber)
    {
        UserData userData = await _userRepository.GetAsync(x => x.Id == id);
        userData.VerifiedPhoneNumber(phoneNumber);
        var result = await _userRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task<UserDto> VerifiedEmailAsync(Guid id, string email)
    {
        UserData userData = await _userRepository.GetAsync(x => x.Id == id);
        userData.VerifiedEmail(email);
        var result = await _userRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task<UserDto> UpdateUserNameAsync(Guid id, UpdateUserNameDto input)
    {
        UserData userData = await _userRepository.GetAsync(x => x.Id == id);
        userData.SetName(input.Name);
        var result = await _userRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }

    public async Task<UserDto> UpdateAccountAsync(Guid id, UpdateAccountDto input)
    {
        UserData userData = await _userRepository.GetAsync(x => x.Id == id);
        userData.SetAccount(input.Account);
        var result = await _userRepository.UpdateAsync(userData);
        return ObjectMapper.Map<UserData, UserDto>(result);
    }
}
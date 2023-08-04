using System;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Stargazer.Abp.Account.Application.Contracts.Users;
using Stargazer.Abp.Account.Application.Contracts.Users.Dtos;
using Lemon.Common.Extend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Caching;
using Volo.Abp.Users;

namespace Stargazer.Abp.Account.HttpApi.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/account")]
public class AccountController : AbpController
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private readonly ICurrentUser _currentUser;
    private readonly IDistributedCache<string> _cache;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        IUserService userService,
        IConfiguration configuration, ICurrentUser currentUser, IDistributedCache<string> cache, ILogger<AccountController> logger)
    {
        _userService = userService;
        _configuration = configuration;
        _currentUser = currentUser;
        _cache = cache;
        _logger = logger;
    }

    [HttpPost("")]
    public async Task<UserDto> CreateAccount(CreateUserDto input)
    {
        return await _userService.CreateAsync(input);
    }

    [HttpPost("verify-email")]
    public async Task VerifyEmailAsync(VerifyEmailDto input)
    {
        var user = await _userService.FindByEmailAsync(input.Email);
        if (user == null)
        {
            _logger.LogError($"###VerifyEmailAsync###------{input.Email} not found.");
            throw new UserFriendlyException("email未注册。");
        }

        var verifyToken = await _cache.GetAsync($"VerifyEmail:{user.Email}");
        if (verifyToken != input.Token)
        {
            _logger.LogError($"###VerifyEmailAsync###------{input.Email} verify token error.");
            throw new UserFriendlyException("token已失效。");
        }
        await _userService.VerifiedEmailAsync(user.Id, user.Email);
        await _cache.RemoveAsync($"VerifyEmail:{user.Email}");
    }

    [HttpGet("")]
    [Authorize]
    public async Task<UserDto> GetAsync()
    {
        return await _userService.GetAsync(_currentUser.Id.GetValueOrDefault());
    }

    [HttpPut("name")]
    [Authorize]
    public async Task<UserDto> UpdateUserName([FromBody] UpdateUserNameDto input)
    {
        return await _userService.UpdateUserNameAsync(_currentUser.Id.GetValueOrDefault(), input);
    }

    [HttpPut("password")]
    [Authorize]
    public async Task<UserDto> UpdatePassword([FromBody] UpdateUserPasswordDto input)
    {
        return await _userService.UpdatePasswordAsync(_currentUser.Id.GetValueOrDefault(), input);
    }

    [HttpPut("email")]
    [Authorize]
    public async Task<UserDto> UpdateEmailAsync([FromBody] UpdateEmailDto input)
    {
        return await _userService.UpdateEmailAsync(_currentUser.Id.GetValueOrDefault(), input.Email);
    }

    [HttpPut("phone")]
    [Authorize]
    public async Task<UserDto> UpdatePhoneNumberAsync([FromBody] UpdatePhoneNumberDto input)
    {
        return await _userService.UpdatePhoneNumberAsync(_currentUser.Id.GetValueOrDefault(), input.PhoneNumber);
    }

    [HttpPost("find-password")]
    public async Task FindPassword(FindPasswordDto input)
    {
        await _userService.FindPasswordAsync(input);
    }

    [HttpPut("reset-password")]
    public async Task ResetPasswordAsync(ResetPasswordDto input)
    {
        await _userService.ResetPasswordAsync(input);
    }
}
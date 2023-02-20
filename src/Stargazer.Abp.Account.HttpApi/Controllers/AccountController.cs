using System;
using System.Linq;
using System.Threading.Tasks;
using Stargazer.Abp.Account.Application.Contracts.Users;
using Stargazer.Abp.Account.Application.Contracts.Users.Dtos;
using Lemon.Common.Extend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
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
    public AccountController(
        IUserService userService, 
        IConfiguration configuration, ICurrentUser currentUser)
    {
        _userService = userService;
        _configuration = configuration;
        _currentUser = currentUser;
    }
        
    [HttpPost("")]
    public async Task<UserDto> CreateAccount(CreateUserDto input)
    {
        return await _userService.CreateAsync(input);
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
}
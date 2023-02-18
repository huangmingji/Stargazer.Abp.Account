using System;
using System.Threading.Tasks;
using Stargazer.Abp.Account.Application.Contracts.Authorization;
using Stargazer.Abp.Account.Application.Contracts.Permissions;
using Stargazer.Abp.Account.Application.Contracts.Users;
using Stargazer.Abp.Account.Application.Contracts.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Stargazer.Abp.Account.HttpApi.Controllers
{
    [Route("api/user")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : AbpController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost("")]
        [Authorize(AccountPermissions.User.Create)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateAsync([FromBody]UpdateUserDto input)
        {
            var userDto = await _userService.CreateAsync(input);
            return Ok(userDto);
        }


        [HttpGet("")]
        [Authorize(AccountPermissions.User.Manage)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResultDto<UserDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetByPage(int pageIndex, int pageSize, string name = null, 
            string account = null, string email = null, string mobile = null)
        {
            var result = await _userService.GetListAsync(pageIndex, pageSize, name, account, email, mobile);
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        [Authorize(AccountPermissions.User.Manage)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var userDto = await _userService.GetAsync(id);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        [Authorize(AccountPermissions.User.Update)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateUserDto data)
        {
            var userDto = await _userService.UpdateUserAsync(id, data);
            return Ok(userDto);
        }
        
        [HttpDelete("{id}")]
        [Authorize(AccountPermissions.User.Delete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task DeleteAsync(Guid id)
        {
            await _userService.DeleteAsync(id);
        }
    }
}

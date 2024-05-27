using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stargazer.Abp.Account.Application.Contracts.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Stargazer.Abp.Account.Application.Contracts.Permissions;
using Stargazer.Abp.Account.Application.Contracts.Roles;
using Stargazer.Abp.Account.Application.Contracts.Roles.Dtos;
using Microsoft.AspNetCore.Http;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Http;

namespace Stargazer.Abp.Account.HttpApi.Controllers
{
    [Route("api/role")]
    [Produces("application/json")]
    [ApiController]
    public class RoleController : AbpController
    {
        private readonly IRoleService _roleService;
        public RoleController(
            IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("")]
        [Authorize(AccountPermissions.Role.Create)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(RemoteServiceErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(RemoteServiceErrorResponse))]
        public async Task<IActionResult> CreateAsync([FromBody]UpdateRoleDto data)
        {
            var result = await _roleService.CreatePublicAsync(data);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(AccountPermissions.Role.Delete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task DeleteAsync(Guid id)
        {
            await _roleService.DeleteAsync(id);
        }

        [HttpGet("{id}")]
        [Authorize(AccountPermissions.Role.Manage)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(RemoteServiceErrorResponse))]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = await _roleService.GetAsync(id);
            return Ok(result);
        }
        
        [HttpGet("")]
        [Authorize(AccountPermissions.Role.Manage)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RoleDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Get()
        {
            var result = await _roleService.GetListAsync();
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(AccountPermissions.Role.Update)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RoleDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(RemoteServiceErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(RemoteServiceErrorResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(RemoteServiceErrorResponse))]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody]UpdateRoleDto data)
        {
            var result = await _roleService.UpdateAsync(id, data);
            return Ok(result);
        }
    }
}

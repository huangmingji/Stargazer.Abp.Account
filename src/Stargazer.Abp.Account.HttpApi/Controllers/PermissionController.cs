using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stargazer.Abp.Account.Application.Contracts.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Stargazer.Abp.Account.Application.Contracts.Permissions;
using Stargazer.Abp.Account.Application.Contracts.Permissions.Dtos;
using Microsoft.AspNetCore.Http;
using Volo.Abp.AspNetCore.Mvc;

namespace Stargazer.Abp.Account.HttpApi.Controllers
{
    [Route("api/permission")]
    [Produces("application/json")]
    [ApiController]
    public class PermissionController : AbpController
    {
        private readonly IPermissionService _permissionService;
        public PermissionController(
            IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpPost("")]
        [Authorize(AccountPermissions.Permission.Create)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PermissionDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateAsync([FromBody]UpdatePermissionDto data)
        {
            var result = await _permissionService.CreateAsync(data);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(AccountPermissions.Permission.Delete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task DeleteAsync(Guid id)
        {
            await _permissionService.DeleteAsync(id);
        }

        [HttpGet("{id}")]
        [Authorize(AccountPermissions.Permission.Manage)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PermissionDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var result = await _permissionService.GetAsync(id);
            return Ok(result);
        }

        [HttpGet("")]
        [Authorize(AccountPermissions.Permission.Manage)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PermissionDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _permissionService.GetListAsync();
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(AccountPermissions.Permission.Update)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PermissionDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody]UpdatePermissionDto data)
        {
            var result = await _permissionService.UpdateAsync(id, data);
            return Ok(result);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stargazer.Abp.Account.Application.Contracts.Roles.Dtos;
using Volo.Abp.Application.Services;

namespace Stargazer.Abp.Account.Application.Contracts.Roles
{
    public interface IRoleService : IApplicationService
    {
        Task<RoleDto> CreatePrivateAsync(UpdateRoleDto input);
        
        Task<RoleDto> CreatePublicAsync(UpdateRoleDto input);

        Task<RoleDto> UpdateAsync(Guid id, UpdateRoleDto input);

        Task<List<RoleDto>> GetListAsync();

        Task<RoleDto> GetAsync(Guid id);
        
        Task<RoleDto> FindAsync(string name);

        Task DeleteAsync(Guid id);
    }
}
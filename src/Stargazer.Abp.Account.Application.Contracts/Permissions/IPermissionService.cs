using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stargazer.Abp.Account.Application.Contracts.Permissions.Dtos;
using Volo.Abp.Application.Services;

namespace Stargazer.Abp.Account.Application.Contracts.Permissions
{
    public interface IPermissionService : IApplicationService
    {
        Task<PermissionDto> CreateAsync(UpdatePermissionDto input);

        Task<PermissionDto> UpdateAsync(Guid id, UpdatePermissionDto input);

        Task<PermissionDto> GetAsync(Guid id);
        
        Task<PermissionDto> FindAsync(string permission);

        Task DeleteAsync(Guid id);

        Task<List<PermissionDto>> GetListAsync();
    }
}
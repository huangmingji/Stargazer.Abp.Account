using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stargazer.Abp.Account.Application.Contracts.Permissions;
using Stargazer.Abp.Account.Application.Contracts.Permissions.Dtos;
using Stargazer.Abp.Account.Domain.Role;
using Lemon.Common.Extend;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Stargazer.Abp.Account.Application.Services
{
    public class PermissionService : ApplicationService, IPermissionService
    {
        private readonly IRepository<PermissionData, Guid> _permissionRepository;
        public PermissionService(IRepository<PermissionData, Guid> permissionRepository)
        {
            this._permissionRepository = permissionRepository;
        }

        public async Task<PermissionDto> CreateAsync(UpdatePermissionDto input)
        {
            await CheckNotNull(input);
            PermissionData permissionData = new PermissionData(
                GuidGenerator.Create(), 
                input.Name, 
                input.Permission, 
                input.ParentId);
            var result = await _permissionRepository.InsertAsync(permissionData);
            return ObjectMapper.Map<PermissionData, PermissionDto>(result);
        }

        public async Task<PermissionDto> FindAsync(string permission)
        {
            var data = await _permissionRepository.FindAsync(x => x.Permission == permission);
            return ObjectMapper.Map<PermissionData, PermissionDto>(data);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _permissionRepository.DeleteAsync(x=> x.Id == id);
        }

        public async Task<PermissionDto> GetAsync(Guid id)
        {
            var data = await _permissionRepository.GetAsync(x => x.Id == id);
            return ObjectMapper.Map<PermissionData, PermissionDto>(data);
        }

        public async Task<List<PermissionDto>> GetListAsync()
        {
            var data = await _permissionRepository.GetListAsync();
            return ObjectMapper.Map<List<PermissionData>, List<PermissionDto>>(data);
        }

        public async Task<PermissionDto> UpdateAsync(Guid id, UpdatePermissionDto input)
        {
            await CheckNotNull(input, id);
            var permissionData = await _permissionRepository.GetAsync(x => x.Id == id);
            permissionData.Set(input.Name, input.Permission, input.ParentId);
            var result = await _permissionRepository.UpdateAsync(permissionData);
            return ObjectMapper.Map<PermissionData, PermissionDto>(result);
        }

        private async Task CheckNotNull(UpdatePermissionDto input, Guid? id = null)
        {
            var expression = Expressionable.Create<PermissionData>()
                            .And(x=> x.Name == input.Name || x.Permission == input.Permission)
                            .And(x=> x.ParentId == input.ParentId)
                            .AndIf(id!= null, x=> x.Id != id);
            if(await _permissionRepository.AnyAsync(expression))
            {
                throw new UserFriendlyException("权限已存在");
            }
        }
    }
}
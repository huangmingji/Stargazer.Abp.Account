using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Stargazer.Abp.Account.Application.Contracts.Roles;
using Stargazer.Abp.Account.Application.Contracts.Roles.Dtos;
using Stargazer.Abp.Account.Domain.Repository;
using Stargazer.Abp.Account.Domain.Role;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Services;

namespace Stargazer.Abp.Account.Application.Services
{
    public class RoleService : ApplicationService, IRoleService
    {
        private IRoleRepository RoleRepository => this.LazyServiceProvider.LazyGetRequiredService<IRoleRepository>();

        public async Task<RoleDto> CreatePrivateAsync(UpdateRoleDto input)
        {
            await RoleRepository.CheckNotNull(input.Name);
            var roleData = new RoleData(
                GuidGenerator.Create(), input.Name, input.IsDefault, false, false);
            input.PermissionIds?.ForEach(item =>
            {
                roleData.Permissions.Add(new RolePermissionData(GuidGenerator.Create(), roleData.Id, item));
            });
            
            if (input.IsDefault)
            {
                var defaultRole = await RoleRepository.FindAsync(x => x.IsDefault);
                if (defaultRole != null)
                {
                    defaultRole.IsDefault = false;
                    await RoleRepository.UpdateAsync(defaultRole);
                }
            }

            var result = await RoleRepository.InsertAsync(roleData);
            return ObjectMapper.Map<RoleData, RoleDto>(result);
        }

        public async Task<RoleDto> CreatePublicAsync(UpdateRoleDto input)
        {
            await RoleRepository.CheckNotNull(input.Name);
            var roleData = new RoleData(
                GuidGenerator.Create(), input.Name, input.IsDefault, false, true);
            input.PermissionIds?.ForEach(item =>
            {
                roleData.Permissions.Add(new RolePermissionData(GuidGenerator.Create(), roleData.Id, item));
            });
            
            if (input.IsDefault)
            {
                var defaultRole = await RoleRepository.FindAsync(x => x.IsDefault);
                if (defaultRole != null)
                {
                    defaultRole.IsDefault = false;
                    await RoleRepository.UpdateAsync(defaultRole);
                }
            }

            var result = await RoleRepository.InsertAsync(roleData);
            return ObjectMapper.Map<RoleData, RoleDto>(result);
        }

        public async Task<RoleDto> FindAsync(string name)
        {
            var data = await RoleRepository.FindAsync(x => x.Name == name);
            return ObjectMapper.Map<RoleData, RoleDto>(data);   
        }

        public async Task DeleteAsync(Guid id)
        {
            await RoleRepository.DeleteAsync(x=> x.Id == id);
        }

        public async Task<List<RoleDto>> GetListAsync()
        {
            var data = await RoleRepository.GetListAsync(true);
            return ObjectMapper.Map<List<RoleData>, List<RoleDto>>(data);
        }

        public async Task<RoleDto> GetAsync(Guid id)
        {
            await RoleRepository.GetAsync(id);
            var data = await RoleRepository.GetAsync(x => x.Id == id);
            return ObjectMapper.Map<RoleData, RoleDto>(data);       
        }

        public async Task<RoleDto> UpdateAsync(Guid id ,UpdateRoleDto input)
        {
            await RoleRepository.CheckNotNull(input.Name, id);
            var roleData = await RoleRepository.GetAsync(x => x.Id == id);
            roleData.Name = input.Name;
            roleData.Permissions.RemoveAll(x => !input.PermissionIds.Contains(x.PermissionId));
            input.PermissionIds?.ForEach(item => {
                if (!roleData.Permissions.Exists(x => x.PermissionId == item))
                {
                    roleData.Permissions.Add(new RolePermissionData(GuidGenerator.Create(), roleData.Id, item));
                }
            });
            
            if (input.IsDefault)
            {
                var defaultRole = await RoleRepository.FindAsync(x => x.IsDefault);
                if (defaultRole != null)
                {
                    defaultRole.IsDefault = false;
                    await RoleRepository.UpdateAsync(defaultRole);
                }
            }
            
            var result = await RoleRepository.UpdateAsync(roleData);
            return ObjectMapper.Map<RoleData, RoleDto>(roleData);
        }
    }
}

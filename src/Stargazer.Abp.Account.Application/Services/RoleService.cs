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
        private readonly ILogger<RoleService> _logger;
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository,
        ILogger<RoleService> logger)
        {
            this._logger = logger;
            this._roleRepository = roleRepository;
        }

        public async Task<RoleDto> CreatePrivateAsync(UpdateRoleDto input)
        {
            await _roleRepository.CheckNotNull(input.Name);
            var roleData = new RoleData(
                GuidGenerator.Create(), input.Name, input.IsDefault, false, false);
            input.Permissions?.ForEach(item =>
            {
                roleData.Permissions.Add(new RolePermissionData(GuidGenerator.Create(), roleData.Id, item));
            });
            
            if (input.IsDefault)
            {
                var defaultRole = await _roleRepository.FindAsync(x => x.IsDefault);
                if (defaultRole != null)
                {
                    defaultRole.IsDefault = false;
                    await _roleRepository.UpdateAsync(defaultRole);
                }
            }

            var result = await _roleRepository.InsertAsync(roleData);
            return ObjectMapper.Map<RoleData, RoleDto>(result);
        }

        public async Task<RoleDto> CreatePublicAsync(UpdateRoleDto input)
        {
            await _roleRepository.CheckNotNull(input.Name);
            var roleData = new RoleData(
                GuidGenerator.Create(), input.Name, input.IsDefault, false, true);
            input.Permissions?.ForEach(item =>
            {
                roleData.Permissions.Add(new RolePermissionData(GuidGenerator.Create(), roleData.Id, item));
            });
            
            if (input.IsDefault)
            {
                var defaultRole = await _roleRepository.FindAsync(x => x.IsDefault);
                if (defaultRole != null)
                {
                    defaultRole.IsDefault = false;
                    await _roleRepository.UpdateAsync(defaultRole);
                }
            }

            var result = await _roleRepository.InsertAsync(roleData);
            return ObjectMapper.Map<RoleData, RoleDto>(result);
        }

        public async Task<RoleDto> FindAsync(string name)
        {
            var data = await _roleRepository.FindAsync(x => x.Name == name);
            return ObjectMapper.Map<RoleData, RoleDto>(data);   
        }

        public async Task DeleteAsync(Guid id)
        {
            await _roleRepository.DeleteAsync(x=> x.Id == id);
        }

        public async Task<List<RoleDto>> GetListAsync()
        {
            var data = await _roleRepository.GetListAsync(true);
            return ObjectMapper.Map<List<RoleData>, List<RoleDto>>(data);
        }

        public async Task<RoleDto> GetAsync(Guid id)
        {
            await _roleRepository.GetAsync(id);
            var data = await _roleRepository.GetAsync(x => x.Id == id);
            return ObjectMapper.Map<RoleData, RoleDto>(data);       
        }

        public async Task<RoleDto> UpdateAsync(Guid id ,UpdateRoleDto input)
        {
            await _roleRepository.CheckNotNull(input.Name, id);
            var roleData = await _roleRepository.GetAsync(x => x.Id == id);
            roleData.Name = input.Name;
            roleData.Permissions.RemoveAll(x => !input.Permissions.Contains(x.Permission));
            input.Permissions?.ForEach(item => {
                if (!roleData.Permissions.Exists(x => x.Permission == item))
                {
                    roleData.Permissions.Add(new RolePermissionData(GuidGenerator.Create(), roleData.Id, item));
                }
            });
            
            if (input.IsDefault)
            {
                var defaultRole = await _roleRepository.FindAsync(x => x.IsDefault);
                if (defaultRole != null)
                {
                    defaultRole.IsDefault = false;
                    await _roleRepository.UpdateAsync(defaultRole);
                }
            }
            
            var result = await _roleRepository.UpdateAsync(roleData);
            return ObjectMapper.Map<RoleData, RoleDto>(roleData);
        }
    }
}

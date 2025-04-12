using Stargazer.Abp.Account.Application.Contracts.Permissions;
using Stargazer.Abp.Account.Application.Contracts.Permissions.Dtos;
using Stargazer.Abp.Account.Domain.Role;
using Stargazer.Common.Extend;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Stargazer.Abp.Account.Application.Services
{
    public class PermissionService : ApplicationService, IPermissionService
    {
        private IRepository<PermissionData, Guid> PermissionRepository => this.LazyServiceProvider.LazyGetRequiredService<IRepository<PermissionData, Guid>>();

        public async Task<PermissionDto> CreateAsync(UpdatePermissionDto input)
        {
            await CheckNotNull(input);
            PermissionData permissionData = new PermissionData(
                GuidGenerator.Create(), 
                input.Name, 
                input.Permission, 
                input.ParentId);
            var result = await PermissionRepository.InsertAsync(permissionData);
            return ObjectMapper.Map<PermissionData, PermissionDto>(result);
        }

        public async Task<PermissionDto> FindAsync(string permission)
        {
            var data = await PermissionRepository.FindAsync(x => x.Permission == permission);
            return ObjectMapper.Map<PermissionData, PermissionDto>(data);
        }

        public async Task DeleteAsync(Guid id)
        {
            await PermissionRepository.DeleteAsync(x=> x.Id == id);
        }

        public async Task<PermissionDto> GetAsync(Guid id)
        {
            var data = await PermissionRepository.GetAsync(x => x.Id == id);
            return ObjectMapper.Map<PermissionData, PermissionDto>(data);
        }

        public async Task<List<PermissionDto>> GetListAsync()
        {
            var data = await PermissionRepository.GetListAsync();
            return ObjectMapper.Map<List<PermissionData>, List<PermissionDto>>(data);
        }

        public async Task<PermissionDto> UpdateAsync(Guid id, UpdatePermissionDto input)
        {
            await CheckNotNull(input, id);
            var permissionData = await PermissionRepository.GetAsync(x => x.Id == id);
            permissionData.Set(input.Name, input.Permission, input.ParentId);
            var result = await PermissionRepository.UpdateAsync(permissionData);
            return ObjectMapper.Map<PermissionData, PermissionDto>(result);
        }

        private async Task CheckNotNull(UpdatePermissionDto input, Guid? id = null)
        {
            var expression = Expressionable.Create<PermissionData>()
                            .AndIf(true, x=> x.Name == input.Name || x.Permission == input.Permission)
                            .AndIf(true, x=> x.ParentId == input.ParentId)
                            .AndIf(id!= null, x=> x.Id != id);
            if(await PermissionRepository.AnyAsync(expression))
            {
                throw new UserFriendlyException("权限已存在");
            }
        }
    }
}
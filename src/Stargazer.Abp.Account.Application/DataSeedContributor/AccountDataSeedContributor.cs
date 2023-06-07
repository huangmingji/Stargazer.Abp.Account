using Stargazer.Abp.Account.Application.Contracts.Authorization;
using Stargazer.Abp.Account.Domain.Repository;
using Stargazer.Abp.Account.Domain.Role;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Stargazer.Abp.Account.Application.DataSeedContributor;

public class AccountDataSeedContributor : IDataSeedContributor, ITransientDependency
{

    private readonly IRepository<PermissionData, Guid> _permissionRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ICurrentTenant _currentTenant;
    private readonly IGuidGenerator _guidGenerator;
    public AccountDataSeedContributor(IRepository<PermissionData, Guid> permissionRepository, IRoleRepository roleRepository, ICurrentTenant currentTenant, IGuidGenerator guidGenerator)
    {
        _permissionRepository = permissionRepository;
        _roleRepository = roleRepository;
        _currentTenant = currentTenant;
        _guidGenerator = guidGenerator;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(context?.TenantId))
        {
            var permissions = await _permissionRepository.GetListAsync();

            foreach (var item in AccountPermissions.DefaultPermissions())
            {
                var data = permissions.FirstOrDefault(x => x.Permission == item.Permission);
                if (data == null)
                {
                    data = new PermissionData(_guidGenerator.Create(), item.Name, item.Permission);
                    await _permissionRepository.InsertAsync(data);
                }
                Thread.Sleep(200);
                foreach (var permission in item.Permissions)
                {
                    var permissionChild = permissions.FirstOrDefault(x => x.Permission == permission.Permission);
                    if (permissionChild == null)
                    {
                        permissionChild = new PermissionData(_guidGenerator.Create(), permission.Name, permission.Permission, data.Id);
                        await _permissionRepository.InsertAsync(permissionChild);
                    }
                    Thread.Sleep(200);
                }
            }

            var role = await _roleRepository.FindAsync(x => x.Name == "账号管理");
            if (role == null)
            {
                role = new RoleData(_guidGenerator.Create(), "账号管理", false, true, false);
                foreach (var item in AccountPermissions.GetAll())
                {
                    Thread.Sleep(200);
                    role.Permissions.Add(new RolePermissionData(_guidGenerator.Create(), role.Id, item));
                }
                await _roleRepository.InsertAsync(role);
            }
        }
    }
}
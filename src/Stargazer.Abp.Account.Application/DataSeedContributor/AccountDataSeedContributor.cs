using Org.BouncyCastle.Bcpg;
using Stargazer.Abp.Account.Application.Contracts.Authorization;
using Stargazer.Abp.Account.Domain.Repository;
using Stargazer.Abp.Account.Domain.Role;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using static Stargazer.Abp.Account.Application.Contracts.Authorization.AccountPermissions;

namespace Stargazer.Abp.Account.Application.DataSeedContributor;

public class AccountDataSeedContributor : IDataSeedContributor, ITransientDependency
{

    private readonly IRepository<PermissionData, Guid> _permissionRepository;
    private readonly ICurrentTenant _currentTenant;
    private readonly IGuidGenerator _guidGenerator;
    public AccountDataSeedContributor(IRepository<PermissionData, Guid> permissionRepository, ICurrentTenant currentTenant, IGuidGenerator guidGenerator)
    {
        _permissionRepository = permissionRepository;
        _currentTenant = currentTenant;
        _guidGenerator = guidGenerator;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(context?.TenantId))
        {
            var permissions = await _permissionRepository.GetListAsync();

            var AccountPermissions = PermissionDefinitionProvider.PermissionGroupDefinitions.Where(x => x.Name == User.Manage || x.Name == Role.Manage || x.Name == Permission.Manage);
            foreach (var item in AccountPermissions)
            {
                var data = permissions.FirstOrDefault(x => x.Permission == item.Name);
                if (data == null)
                {
                    data = new PermissionData(_guidGenerator.Create(), item.DisplayName, item.Name);
                    await _permissionRepository.InsertAsync(data);
                }
                Thread.Sleep(200);
                foreach (var permission in item.PermissionDefinitions)
                {
                    var permissionChild = permissions.FirstOrDefault(x => x.Permission == permission.Name);
                    if (permissionChild == null)
                    {
                        permissionChild = new PermissionData(_guidGenerator.Create(), permission.DisplayName, permission.Name, data.Id);
                        await _permissionRepository.InsertAsync(permissionChild);
                    }
                    Thread.Sleep(200);
                }
            }
        }
    }
}
using Stargazer.Abp.Account.Domain.Role;
using Stargazer.Abp.Account.Domain.Shared;
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
            
            {
                var data = permissions.FirstOrDefault(x => x.Permission == User.Manage);
                if (data == null)
                {
                    data = new PermissionData(_guidGenerator.Create(), LocalizationExtension.L("UserManagement").Name, User.Manage);
                    await _permissionRepository.InsertAsync(data);
                }

                var createUser = permissions.FirstOrDefault(x => x.Permission == User.Create);
                if (createUser == null)
                {
                    createUser = new PermissionData(_guidGenerator.Create(), LocalizationExtension.L("CreateUser").Name, User.Create, data.Id);
                    await _permissionRepository.InsertAsync(createUser);
                }

                var updateUser = permissions.FirstOrDefault(x => x.Permission == User.Update);
                if (updateUser == null)
                {
                    updateUser = new PermissionData(_guidGenerator.Create(), LocalizationExtension.L("UpdateUser").Name, User.Update, data.Id);
                    await _permissionRepository.InsertAsync(updateUser);
                }

                var deleteUser = permissions.FirstOrDefault(x => x.Permission == User.Delete);
                if (deleteUser == null)
                {
                    deleteUser = new PermissionData(_guidGenerator.Create(), LocalizationExtension.L("DeleteUser").Name, User.Delete, data.Id);
                    await _permissionRepository.InsertAsync(deleteUser);
                }
            }

            {
                var data = permissions.FirstOrDefault(x => x.Permission == Role.Manage);
                if (data == null)
                {
                    data = new PermissionData(_guidGenerator.Create(), LocalizationExtension.L("RoleManagement").Name, Role.Manage);
                    await _permissionRepository.InsertAsync(data);
                }

                var createRole = permissions.FirstOrDefault(x => x.Permission == Role.Create);
                if (createRole == null)
                {
                    createRole = new PermissionData(_guidGenerator.Create(), LocalizationExtension.L("CreateRole").Name, Role.Create, data.Id);
                    await _permissionRepository.InsertAsync(createRole);
                }

                var updateRole = permissions.FirstOrDefault(x => x.Permission == Role.Update);
                if (updateRole == null)
                {
                    updateRole = new PermissionData(_guidGenerator.Create(), LocalizationExtension.L("UpdateRole").Name, Role.Update, data.Id);
                    await _permissionRepository.InsertAsync(updateRole);
                }

                var deleteRole = permissions.FirstOrDefault(x => x.Permission == Role.Delete);
                if (deleteRole == null)
                {
                    deleteRole = new PermissionData(_guidGenerator.Create(), LocalizationExtension.L("DeleteRole").Name, Role.Delete, data.Id);
                    await _permissionRepository.InsertAsync(deleteRole);
                }
            }

            {
                var data = permissions.FirstOrDefault(x=> x.Permission == Permission.Manage);
                if (data == null)
                {
                    data = new PermissionData(_guidGenerator.Create(), LocalizationExtension.L("PermissionManagement").Name, Permission.Manage);
                    await _permissionRepository.InsertAsync(data);
                }

                var createPermission = permissions.FirstOrDefault(x=> x.Permission == Permission.Create);
                if (createPermission == null)
                {
                    createPermission = new PermissionData(_guidGenerator.Create(), LocalizationExtension.L("CreatePermission").Name, Permission.Create, data.Id);
                    await _permissionRepository.InsertAsync(createPermission);
                }

                var updatePermission = permissions.FirstOrDefault(x=> x.Permission == Permission.Update);
                if (updatePermission == null)
                {
                    updatePermission = new PermissionData(_guidGenerator.Create(), LocalizationExtension.L("UpdatePermission").Name, Permission.Update, data.Id);
                    await _permissionRepository.InsertAsync(updatePermission);
                }

                var deletePermission = permissions.FirstOrDefault(x=> x.Permission == Permission.Delete);
                if (deletePermission == null)
                {
                    deletePermission = new PermissionData(_guidGenerator.Create(), LocalizationExtension.L("DeletePermission").Name, Permission.Delete, data.Id);
                    await _permissionRepository.InsertAsync(deletePermission);
                }

            }
        }
    }
}
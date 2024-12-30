using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stargazer.Abp.Account.Application.Contracts.Authorization;
using Stargazer.Abp.Account.Application.Contracts.Permissions;
using Stargazer.Abp.Account.Application.Contracts.Roles;
using Stargazer.Abp.Account.Application.Contracts.Roles.Dtos;
using Stargazer.Abp.Account.Application.Contracts.Users;
using Stargazer.Abp.Account.Application.Contracts.Users.Dtos;
using Volo.Abp.DependencyInjection;

namespace Stargazer.Abp.Account.DbMigrations
{
    public class DbMigrationService : ITransientDependency
    {
        private IServiceProvider _serviceProvider;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;
        public DbMigrationService(
            IServiceProvider serviceProvider,
            IUserService userService,
            IRoleService roleService,
            IPermissionService permissionService)
        {
            _serviceProvider = serviceProvider;
            _userService = userService;
            _roleService = roleService;
            _permissionService = permissionService;
        }

        public async Task MigrateAsync()
        {
            try
            {
                var permissions = await _permissionService.GetListAsync();
                var role = await _roleService.FindAsync("超级管理员");
                if (role == null)
                {
                    var adminRole = new UpdateRoleDto()
                    {
                        Name = "超级管理员",
                        IsDefault = false
                    };

                    var permissionIds = permissions.Where(x => AccountPermissions.GetAll().Contains(x.Permission)).ToList().ConvertAll(x => x.Id);
                    adminRole.PermissionIds.AddRange(permissionIds);
                    role = await _roleService.CreatePrivateAsync(adminRole);
                }

                var user = await _userService.FindByEmailAsync("admin@yunpan.com");
                if (user == null)
                {
                    var userInput = new CreateOrUpdateUserWithRolesDto()
                    {
                        UserName = "admin",
                        Password = "Password123456",
                        Email = "admin@yunpan.com",
                        RoleIds = new List<Guid>() { role.Id }
                    };
                    await _userService.CreateAsync(userInput);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            // var dbMigrationsDbContext = _serviceProvider.GetService<DbMigrationsDbContext>();
            // if(dbMigrationsDbContext != null)
            // {
            //     await dbMigrationsDbContext.Database.MigrateAsync();
            // }
            //
            // await InitDataAsync();
        }
    }
}
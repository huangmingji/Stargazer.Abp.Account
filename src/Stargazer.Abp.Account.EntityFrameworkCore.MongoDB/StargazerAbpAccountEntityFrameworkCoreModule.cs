using Stargazer.Abp.Account.Domain;
using Stargazer.Abp.Account.Domain.Users;
using Microsoft.Extensions.DependencyInjection;
using Stargazer.Abp.Account.Domain.Role;
using Stargazer.Abp.Account.EntityFrameworkCore.MongoDB.Repository;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace Stargazer.Abp.Account.EntityFrameworkCore.MongoDB
{
    [DependsOn(
        typeof(StargazerAbpAccountDomainModule),
        typeof(AbpMongoDbModule))]
    public class StargazerAbpAccountEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<AccountDbContext>(options => {
                options.AddDefaultRepositories(includeAllEntities: true);
                options.AddRepository<UserData, UserRepository>();
                options.AddRepository<RoleData, RoleRepository>();
            });
        }
    }
}
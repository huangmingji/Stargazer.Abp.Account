using Stargazer.Abp.Account.Domain;
using Stargazer.Abp.Account.Domain.Users;
using Stargazer.Abp.Account.EntityFrameworkCore.Repository;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace Stargazer.Abp.Account.EntityFrameworkCore
{
    [DependsOn(
        typeof(StargazerAbpAccountDomainModule),
        typeof(AbpEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule))]
    public class StargazerAbpAccountEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AccountDbContext>(options => {
                options.AddDefaultRepositories(includeAllEntities: true);
                options.AddRepository<UserData, UserRepository>();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
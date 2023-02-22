using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Stargazer.Abp.Account.EntityFrameworkCore.DbMigrations
{
    [DependsOn(
        typeof(StargazerAbpAccountEntityFrameworkCoreModule))]
    public class StargazerAbpAccountEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<AccountDbMigrationsDbContext>(options => {
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });

            #region 自动迁移数据库

            var  accountDbMigrationsDbContext =  context.Services.BuildServiceProvider().GetService<AccountDbMigrationsDbContext>();
            if (accountDbMigrationsDbContext != null)
            {
                accountDbMigrationsDbContext.Database.Migrate();
            }
            
            #endregion 自动迁移数据库
        }
    }
}
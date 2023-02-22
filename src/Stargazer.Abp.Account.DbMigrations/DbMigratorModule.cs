using Stargazer.Abp.Account.Application;
using Stargazer.Abp.Account.EntityFrameworkCore.DbMigrations;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Stargazer.Abp.Account.DbMigrations
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(StargazerAbpAccountEntityFrameworkCoreDbMigrationsModule),
        typeof(StargazerAbpAccountApplicationModule)
    )]
    public class DbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
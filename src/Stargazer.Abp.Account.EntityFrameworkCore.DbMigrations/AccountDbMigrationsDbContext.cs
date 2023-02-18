using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Stargazer.Abp.Account.EntityFrameworkCore.DbMigrations
{    
    [ConnectionStringName("Default")]
    public class AccountDbMigrationsDbContext: AbpDbContext<AccountDbMigrationsDbContext>
    {
        public AccountDbMigrationsDbContext(DbContextOptions<AccountDbMigrationsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ConfigureLemonAccount();
            base.OnModelCreating(builder);
        }

    }
}
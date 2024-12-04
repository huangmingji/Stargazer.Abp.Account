using Stargazer.Abp.Account.Domain.Role;
using Stargazer.Abp.Account.Domain.Users;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Stargazer.Abp.Account.EntityFrameworkCore.MongoDB;

public static class StargazerAbpAccountDbContextModelCreatingExtensions
{
    public static void ConfigureStargazerAbpAccount(this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
        builder.ConfigureAccount();
    }


    private static void ConfigureAccount(this IMongoModelBuilder builder)
    {
        builder.Entity<RoleData>(b =>
        {
            b.CollectionName = "RoleDatas";
        });

        builder.Entity<RolePermissionData>(b =>
        {
            b.CollectionName = "RolePermissionDatas";
        });

        builder.Entity<PermissionData>(b =>
        {
            b.CollectionName = "PermissionDatas";
        });

        builder.Entity<UserData>(b =>
        {
            b.CollectionName = "UserDatas";
        });

        builder.Entity<UserRole>(b =>
        {
            b.CollectionName = "UserRoles";
        });
    }
}
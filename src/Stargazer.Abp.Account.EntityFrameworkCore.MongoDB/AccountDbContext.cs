using MongoDB.Driver;
using Stargazer.Abp.Account.Domain.Role;
using Stargazer.Abp.Account.Domain.Users;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Stargazer.Abp.Account.EntityFrameworkCore.MongoDB;

[ConnectionStringName("MongoDb")]
public class AccountDbContext : AbpMongoDbContext
{
    public IMongoCollection<UserData> UserDatas => Collection<UserData>();
    public IMongoCollection<RoleData> RoleDatas => Collection<RoleData>();
    public IMongoCollection<PermissionData> PermissionDatas => Collection<PermissionData>();
    public IMongoCollection<UserRole> UserRoles => Collection<UserRole>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);
        modelBuilder.ConfigureStargazerAbpAccount();
    }
}

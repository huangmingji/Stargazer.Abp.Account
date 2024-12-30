using Stargazer.Abp.Account.Domain.Repository;
using Stargazer.Abp.Account.Domain.Role;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Stargazer.Abp.Account.EntityFrameworkCore.MongoDB.Repository;

public class RoleRepository : MongoDbRepository<AccountDbContext, RoleData, Guid>, IRoleRepository
{
    public RoleRepository(IMongoDbContextProvider<AccountDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task CheckNotNull(string name, Guid? id = null)
    {
        var queryable = await GetQueryableAsync();
        if (queryable.Where(x => x.Name == name).WhereIf(id != null, x => x.Id != id).Any())
        {
            throw new RoleNotNullException(id, name);
        }
    }
}
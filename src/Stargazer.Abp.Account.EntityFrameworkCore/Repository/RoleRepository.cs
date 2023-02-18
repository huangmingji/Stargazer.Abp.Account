using System;
using System.Linq;
using System.Threading.Tasks;
using Stargazer.Abp.Account.Domain.Repository;
using Stargazer.Abp.Account.Domain.Role;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Stargazer.Abp.Account.EntityFrameworkCore.Repository;

public class RoleRepository : EfCoreRepository<AccountDbContext, RoleData, Guid>, IRoleRepository
{
    public RoleRepository(IDbContextProvider<AccountDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<RoleData>> WithDetailsAsync()
    {
        var queryable = await GetQueryableAsync();
        return queryable.Include(x => x.Permissions);
    }
    
    public async Task CheckNotNull(string name, Guid? id = null)
    {
        var queryable = await GetQueryableAsync();
        if (queryable.Where(x => x.Name == name).WhereIf(id != null, x => x.Id == id).Any())
        {
            throw new RoleNotNullException(id, name);
        }
    }
}
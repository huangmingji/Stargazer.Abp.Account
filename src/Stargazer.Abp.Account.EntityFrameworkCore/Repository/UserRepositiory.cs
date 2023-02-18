using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Stargazer.Abp.Account.Domain.Repository;
using System.Threading.Tasks;
using Stargazer.Abp.Account.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Stargazer.Abp.Account.EntityFrameworkCore.Repository
{
    public class UserRepository : EfCoreRepository<AccountDbContext, UserData, Guid>, IUserRepository
    {
        public UserRepository(IDbContextProvider<AccountDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public override async Task<IQueryable<UserData>> WithDetailsAsync()
        {
            var queryable = await GetQueryableAsync();
            return queryable.Include(x => x.UserRoles)
                .ThenInclude(x => x.RoleData)
                .ThenInclude(x => x.Permissions);
        }

        public async Task<List<UserData>> GetListByPageAsync(Expression<Func<UserData, bool>> expression, int pageIndex,
            int pageSize)
        {
            var queryable = await GetQueryableAsync();
            var data = queryable.Where(expression)
                .OrderByDescending(x => x.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return data;
        }
    }
}
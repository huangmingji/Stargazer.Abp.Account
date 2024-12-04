using System.Linq.Expressions;
using Stargazer.Abp.Account.Domain.Repository;
using Stargazer.Abp.Account.Domain.Users;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Stargazer.Abp.Account.EntityFrameworkCore.MongoDB.Repository
{
    public class UserRepository : MongoDbRepository<AccountDbContext, UserData, Guid>, IUserRepository
    {
        public UserRepository(IMongoDbContextProvider<AccountDbContext> dbContextProvider) : base(dbContextProvider)
        {
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
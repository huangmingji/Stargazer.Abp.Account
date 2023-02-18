using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Stargazer.Abp.Account.Domain.Users;
using Volo.Abp.Domain.Repositories;

namespace Stargazer.Abp.Account.Domain.Repository
{
    public interface IUserRepository : IRepository<UserData, Guid>
    {
        Task<List<UserData>> GetListByPageAsync(Expression<Func<UserData, bool>> expression, int pageIndex, int pageSize);
    }
}
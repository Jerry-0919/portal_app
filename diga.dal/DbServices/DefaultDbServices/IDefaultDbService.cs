using diga.bl.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.DefaultDbServices
{
    public interface IDefaultDbService<TKey, TEntity>
           where TKey : IEquatable<TKey>
           where TEntity : class, IDbServiceEntity<TKey>
    {
        Task<bool> Any(TKey id);
        Task<int> Count();
        Task<TEntity> Get(TKey id);
        Task<List<TEntity>> List();

        Task<TEntity> Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);

        Task Update(TEntity entity);
        Task UpdateRange(IEnumerable<TEntity> entities);

        Task Remove(TEntity entity);
        Task RemoveRange(IEnumerable<TEntity> entities);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.ManyToManyDbServices
{
    public interface IManyToManyDbService<TEntity>
    {
        Task<List<TEntity>> List();
        Task<TEntity> Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        Task Update(TEntity entity);
        Task UpdateRange(IEnumerable<TEntity> entities);
        Task Remove(TEntity entity);
        Task RemoveRange(IEnumerable<TEntity> entities);
    }
}

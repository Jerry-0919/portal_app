using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.ManyToManyDbServices
{
    public class ManyToManyDbService<TEntity> : IManyToManyDbService<TEntity>
        where TEntity : class
    {
        private readonly DbContext _db;

        public ManyToManyDbService(DbContext db)
        {
            _db = db;
        }

        public async Task<List<TEntity>> List()
        {
            return await _db.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            EntityEntry entry = await _db.Set<TEntity>().AddAsync(entity);
            await _db.SaveChangesAsync();

            return (TEntity)entry.Entity;
        }

        public async Task AddRange(IEnumerable<TEntity> entities)
        {
            await _db.Set<TEntity>().AddRangeAsync(entities);
            await _db.SaveChangesAsync();
        }

        public async Task Update(TEntity entity)
        {
            _db.Set<TEntity>().Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateRange(IEnumerable<TEntity> entities)
        {
            _db.Set<TEntity>().UpdateRange(entities);
            await _db.SaveChangesAsync();
        }

        public async Task Remove(TEntity entity)
        {
            _db.Set<TEntity>().Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveRange(IEnumerable<TEntity> entities)
        {
            _db.Set<TEntity>().RemoveRange(entities);
            await _db.SaveChangesAsync();
        }
    }
}

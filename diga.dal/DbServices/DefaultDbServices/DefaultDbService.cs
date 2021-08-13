using diga.bl.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.DefaultDbServices
{
    public class DefaultDbService<TKey, TEntity> : IDefaultDbService<TKey, TEntity>
        where TKey : IEquatable<TKey>
        where TEntity : class, IDbServiceEntity<TKey>
    {
        private readonly DbContext _db;

        public DefaultDbService(DbContext db)
        {
            _db = db;
        }

        public async Task<bool> Any(TKey id)
        {
            return await _db.Set<TEntity>().AnyAsync(e => e.Id.Equals(id));
        }

        public async Task<int> Count()
        {
            return await _db.Set<TEntity>().CountAsync();
        }

        public async Task<List<TEntity>> List()
        {
            return await _db.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> Get(TKey id)
        {
            return await _db.Set<TEntity>().OrderBy(e => e.Id)
                .FirstOrDefaultAsync(e => e.Id.Equals(id));
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

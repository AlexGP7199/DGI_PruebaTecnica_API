using DGI_PruebaTecnica.Aplicacion.Base;
using DGI_PruebaTecnica.Infraestructura.Persistences.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Test.Infrastructure
{
    public class TestGenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _set;

        public TestGenericRepository(DbSet<T> set) => _set = set;

      
        public IQueryable<T> GetEntityQuery(Expression<Func<T, bool>>? filter = null)
        {
            var q = _set.Where(x => x.Activo).AsQueryable();
            if (filter != null) q = q.Where(filter);
            return q;
        }

        public IQueryable<T> GetAllQueryable() => _set.Where(x => x.Activo).AsNoTracking();

        
        public async Task<IEnumerable<T>> GetllAsync() =>
            await _set.AsNoTracking().Where(x => x.Activo).ToListAsync();

        public Task<T> GetByIdAsync(int id) =>
            _set.FirstOrDefaultAsync(x => x.Id == id && x.Activo)!;

        public Task<T> GetByIdIncludingDeletedAsync(int id) =>
            _set.FirstOrDefaultAsync(x => x.Id == id)!;

        public async Task<bool> RegisterAsync(T entity)
        {
            entity.Activo = true;
            await _set.AddAsync(entity);
            return true;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _set.Update(entity);
            return await Task.FromResult(true);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var e = await _set.FindAsync(id);
            if (e is null) return false;
            e.Activo = false;
            _set.Update(e);
            return true;
        }

        public async Task<bool> RegisterMultipleAsync(IEnumerable<T> entities)
        {
            foreach (var e in entities) e.Activo = true;
            await _set.AddRangeAsync(entities);
            return true;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<T> entities)
        {
            foreach (var e in entities) e.Activo = true;
            await _set.AddRangeAsync(entities);
            return true;
        }

        public Task<bool> RemoveRange(IEnumerable<T> entities)
        {
            foreach (var e in entities) { e.Activo = false; _set.Update(e); }
            return Task.FromResult(true);
        }
    }
}

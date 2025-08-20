using DGI_PruebaTecnica.Aplicacion.Base;
using DGI_PruebaTecnica.Infraestructura.Persistences.Context;
using DGI_PruebaTecnica.Infraestructura.Persistences.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Infraestructura.Persistences.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _entity;
        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _entity = _dbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetllAsync()
        {
            var getAll = await _entity.AsNoTracking().Where(x => x.Activo).ToListAsync();
            return getAll;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var getById = await _entity.FirstOrDefaultAsync(x => x.Id.Equals(id) && x.Activo);

            return getById!;
        }

        public async Task<T> GetByIdIncludingDeletedAsync(int id)
        {
            return await _entity.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }


        public async Task<bool> RegisterAsync(T entity)
        {
            entity.Activo = true;
            await _entity.AddAsync(entity);
            var recordsAffected = await _dbContext.SaveChangesAsync(); // ✅ Se asegura de que realmente se guarde
            return recordsAffected > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {

            var existingEntity = await _entity.FindAsync(entity.Id);
            if (existingEntity == null) return false;

            // 🔹 Actualizar solo los campos modificables manualmente
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);

            // 🔹 Marcar la entidad como modificada
            _dbContext.Entry(existingEntity).State = EntityState.Modified;

            var recordsAffected = await _dbContext.SaveChangesAsync();
            return recordsAffected > 0;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var entity = await _entity.FindAsync(id);
            if (entity == null)
                return false;

            if (!entity.Activo)
                throw new InvalidOperationException("La amonestación ya fue desactivada previamente.");

            entity.Activo = false;
            // ... resto igual


            entity.Activo = false; // Marcamos como inactivo

            // 🔹 Desconectamos la entidad para evitar conflictos de tracking previos
            _dbContext.Entry(entity).State = EntityState.Detached;

            // 🔹 Re-adjuntamos la entidad y marcamos explícitamente como modificada
            _dbContext.Attach(entity);
            _dbContext.Entry(entity).Property(e => e.Activo).IsModified = true;

            var recordsAffected = await _dbContext.SaveChangesAsync();
            return recordsAffected > 0;
        }


        public IQueryable<T> GetEntityQuery(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _entity.Where(x => x.Activo);

            if (filter != null) query = query.Where(filter);

            return query;
        }

        public async Task<bool> RegisterMultipleAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.Activo = true; // Asegurar que todos los registros sean activos
            }

            await _entity.AddRangeAsync(entities); // Agregar en un solo lote
            var recordsAffected = await _dbContext.SaveChangesAsync();

            return recordsAffected > 0;
        }


        public IQueryable<T> GetAllQueryable()
        {
            var getAllQuery = GetEntityQuery().AsNoTracking();
            return getAllQuery;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                entity.Activo = true;

            await _entity.AddRangeAsync(entities);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> RemoveRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.Activo = false;
                _dbContext.Entry(entity).State = EntityState.Modified;
            }

            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}

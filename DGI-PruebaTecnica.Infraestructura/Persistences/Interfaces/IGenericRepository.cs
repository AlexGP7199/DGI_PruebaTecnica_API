using DGI_PruebaTecnica.Aplicacion.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Infraestructura.Persistences.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // Lectura
        IQueryable<T> GetAllQueryable();
        Task<IEnumerable<T>> GetllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdIncludingDeletedAsync(int id); // útil si luego implementas soft delete
        IQueryable<T> GetEntityQuery(Expression<Func<T, bool>>? filter = null);

        // Escritura
        Task<bool> RegisterAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> RemoveAsync(int id);

        // Operaciones por lotes (útiles más adelante)
        Task<bool> RegisterMultipleAsync(IEnumerable<T> entities);
        Task<bool> AddRangeAsync(IEnumerable<T> entities);
        Task<bool> RemoveRange(IEnumerable<T> entities);
    }
}

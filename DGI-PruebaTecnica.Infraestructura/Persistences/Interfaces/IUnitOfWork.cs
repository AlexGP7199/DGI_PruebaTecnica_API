using DGI_PruebaTecnica.Aplicacion.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Infraestructura.Persistences.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Repositorios (genéricos)
        IGenericRepository<Contribuyente> Contribuyentes { get; }
        IGenericRepository<ComprobanteFiscal> ComprobantesFiscales { get; }
        IGenericRepository<TipoContribuyente> TipoContribuyentes { get; }
        IGenericRepository<EstatusContribuyente> EstatusContribuyentes { get; }
        IGenericRepository<TipoNCF> TiposNCF { get; }

        // Transacciones
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        // Persistencia
        void SaveChanges();
        Task SaveChangesAsync();

        // Utilidades de contexto
        void DetachEntity<T>(T entity) where T : class;
    }
}

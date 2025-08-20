using DGI_PruebaTecnica.Aplicacion.Entities;
using DGI_PruebaTecnica.Infraestructura.Persistences.Context;
using DGI_PruebaTecnica.Infraestructura.Persistences.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Infraestructura.Persistences.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private IDbContextTransaction? _currentTransaction;

        public IGenericRepository<Contribuyente> Contribuyentes { get; }
        public IGenericRepository<ComprobanteFiscal> ComprobantesFiscales { get; }
        public IGenericRepository<TipoContribuyente> TipoContribuyentes { get; }
        public IGenericRepository<EstatusContribuyente> EstatusContribuyentes { get; }
        public IGenericRepository<TipoNCF> TiposNCF { get; }

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;

            // Instancias de repos genéricos
            Contribuyentes = new GenericRepository<Contribuyente>(_dbContext);
            ComprobantesFiscales = new GenericRepository<ComprobanteFiscal>(_dbContext);
            TipoContribuyentes = new GenericRepository<TipoContribuyente>(_dbContext);
            EstatusContribuyentes = new GenericRepository<EstatusContribuyente>(_dbContext);
            TiposNCF = new GenericRepository<TipoNCF>(_dbContext);
        }

        public async Task BeginTransactionAsync()
        {
            // Evita abrir más de una transacción a la vez
            if (_currentTransaction is not null) return;
            _currentTransaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction is null) return;

            try
            {
                await _dbContext.SaveChangesAsync(); // asegura persistencia
                await _currentTransaction.CommitAsync();
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction is null) return;

            try
            {
                await _currentTransaction.RollbackAsync();
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        /* ======================
           Persistencia
           ====================== */

        public void SaveChanges() => _dbContext.SaveChanges();

        public Task SaveChangesAsync() => _dbContext.SaveChangesAsync();

        /* ======================
           Utilidades
           ====================== */

        public void DetachEntity<T>(T entity) where T : class
        {
            var entry = _dbContext.Entry(entity);
            if (entry is { State: not EntityState.Detached })
                entry.State = EntityState.Detached;
        }

        /* ======================
           Limpieza
           ====================== */

        public void Dispose()
        {
            _currentTransaction?.Dispose();
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

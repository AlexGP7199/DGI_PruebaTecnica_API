using DGI_PruebaTecnica.Aplicacion.Entities;
using DGI_PruebaTecnica.Infraestructura.Persistences.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Test.Infrastructure
{

    public class TestUnitOfWork : IUnitOfWork
    {
        private readonly TestDbContext _ctx;

        public TestUnitOfWork(TestDbContext ctx)
        {
            _ctx = ctx;

            Contribuyentes = new TestGenericRepository<Contribuyente>(_ctx.Contribuyentes);
            ComprobantesFiscales = new TestGenericRepository<ComprobanteFiscal>(_ctx.ComprobantesFiscales);
            TipoContribuyentes = new TestGenericRepository<TipoContribuyente>(_ctx.TipoContribuyentes);
            EstatusContribuyentes = new TestGenericRepository<EstatusContribuyente>(_ctx.EstatusContribuyentes);
            TiposNCF = new TestGenericRepository<TipoNCF>(_ctx.TiposNCF);
        }

        // Repos
        public IGenericRepository<Contribuyente> Contribuyentes { get; }
        public IGenericRepository<ComprobanteFiscal> ComprobantesFiscales { get; }
        public IGenericRepository<TipoContribuyente> TipoContribuyentes { get; }
        public IGenericRepository<EstatusContribuyente> EstatusContribuyentes { get; }
        public IGenericRepository<TipoNCF> TiposNCF { get; }

        // Transacciones (no‑op en unit tests)
        public Task BeginTransactionAsync() => Task.CompletedTask;
        public Task CommitTransactionAsync() => Task.CompletedTask;
        public Task RollbackTransactionAsync() => Task.CompletedTask;

        // Persistencia (opcional en unit tests; para GETs no hace falta)
        public void SaveChanges() { /* no-op */ }
        public Task SaveChangesAsync() => Task.CompletedTask;

        // Utilidad
        public void DetachEntity<T>(T entity) where T : class
        {
            var entry = _ctx.Entry(entity);
            if (entry is { State: not EntityState.Detached })
                entry.State = EntityState.Detached;
        }

        public void Dispose() => _ctx.Dispose();
    }
}

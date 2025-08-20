using DGI_PruebaTecnica.Aplicacion.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Infraestructura.Persistences.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Contribuyente> Contribuyentes => Set<Contribuyente>();
        public DbSet<ComprobanteFiscal> ComprobantesFiscales => Set<ComprobanteFiscal>();
        public DbSet<TipoContribuyente> TipoContribuyentes => Set<TipoContribuyente>();
        public DbSet<EstatusContribuyente> EstatusContribuyentes => Set<EstatusContribuyente>();
        public DbSet<TipoNCF> TiposNCF => Set<TipoNCF>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}

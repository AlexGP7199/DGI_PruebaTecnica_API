using DGI_PruebaTecnica.Aplicacion.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Test.Infrastructure
{
    // Usa tus entidades reales (mismo namespace de dominio/infra)
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<Contribuyente> Contribuyentes => Set<Contribuyente>();
        public DbSet<ComprobanteFiscal> ComprobantesFiscales => Set<ComprobanteFiscal>();
        public DbSet<TipoContribuyente> TipoContribuyentes => Set<TipoContribuyente>();
        public DbSet<EstatusContribuyente> EstatusContribuyentes => Set<EstatusContribuyente>();
        public DbSet<TipoNCF> TiposNCF => Set<TipoNCF>();
    }

}

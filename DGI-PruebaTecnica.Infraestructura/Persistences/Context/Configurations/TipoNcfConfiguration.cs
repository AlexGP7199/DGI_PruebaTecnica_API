using DGI_PruebaTecnica.Aplicacion.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Infraestructura.Persistences.Context.Configurations
{
    public class TipoNcfConfiguration : IEntityTypeConfiguration<TipoNCF>
    {
        public void Configure(EntityTypeBuilder<TipoNCF> b)
        {
            b.ToTable("TipoNCF");
            b.HasKey(x => x.Id);

            b.Property(x => x.Codigo).HasMaxLength(5).IsRequired();
            b.Property(x => x.Nombre).HasMaxLength(60).IsRequired();
            b.HasIndex(x => x.Codigo).IsUnique();
        }
    }
}

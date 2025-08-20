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
    public class TipoContribuyenteConfiguration : IEntityTypeConfiguration<TipoContribuyente>
    {
        public void Configure(EntityTypeBuilder<TipoContribuyente> b)
        {
            b.ToTable("TipoContribuyente");
            b.HasKey(x => x.Id);

            b.Property(x => x.Nombre).HasMaxLength(20).IsRequired();
            b.HasIndex(x => x.Nombre).IsUnique();
        }
    }
}

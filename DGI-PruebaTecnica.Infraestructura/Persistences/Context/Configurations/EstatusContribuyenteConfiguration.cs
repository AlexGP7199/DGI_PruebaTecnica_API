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
    public class EstatusContribuyenteConfiguration : IEntityTypeConfiguration<EstatusContribuyente>
    {
        public void Configure(EntityTypeBuilder<EstatusContribuyente> b)
        {
            b.ToTable("EstatusContribuyente");
            b.HasKey(x => x.Id);

            b.Property(x => x.Nombre).HasMaxLength(10).IsRequired();
            b.Property(x => x.Activo).HasColumnType("bit").HasDefaultValue(true);
            b.HasIndex(x => x.Nombre).IsUnique();
        }
    }
}

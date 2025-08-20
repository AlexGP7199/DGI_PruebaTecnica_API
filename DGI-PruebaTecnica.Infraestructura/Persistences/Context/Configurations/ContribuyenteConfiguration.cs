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
    public class ContribuyenteConfiguration : IEntityTypeConfiguration<Contribuyente>
    {
        public void Configure(EntityTypeBuilder<Contribuyente> b)
        {
            b.ToTable("Contribuyentes");
            b.HasKey(x => x.Id);

            b.Property(x => x.RncCedula).HasMaxLength(13).IsRequired();
            b.HasIndex(x => x.RncCedula).IsUnique();

            b.Property(x => x.Nombre).HasMaxLength(150).IsRequired();
            b.Property(x => x.FechaCreacion).HasColumnType("datetime2(0)");

            b.HasOne(x => x.TipoContribuyente)
                .WithMany(x => x.Contribuyentes)
                .HasForeignKey(x => x.TipoContribuyenteId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.EstatusContribuyente)
                .WithMany(x => x.Contribuyentes)
                .HasForeignKey(x => x.EstatusContribuyenteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

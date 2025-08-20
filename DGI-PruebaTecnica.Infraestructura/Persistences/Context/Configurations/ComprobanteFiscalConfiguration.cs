using DGI_PruebaTecnica.Aplicacion.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Infraestructura.Persistences.Context.Configurations
{
    public class ComprobanteFiscalConfiguration : IEntityTypeConfiguration<ComprobanteFiscal>
    {
        public void Configure(EntityTypeBuilder<ComprobanteFiscal> b)
        {
            b.ToTable("ComprobantesFiscales");
            b.HasKey(x => x.Id);

            b.Property(x => x.NCF).HasMaxLength(19).IsRequired();

            b.Property(x => x.Monto).HasColumnType("decimal(18,2)").IsRequired();
            b.Property(x => x.TasaItbis).HasColumnType("decimal(5,2)").HasDefaultValue(18.00m).IsRequired();

            b.Property(x => x.Itbis18)
                .HasColumnType("decimal(18,2)")
                .HasComputedColumnSql("CAST(ROUND([Monto] * ([TasaItbis] / 100.0), 2) AS DECIMAL(18,2))", stored: true)
                .ValueGeneratedOnAddOrUpdate();
            b.Property(x => x.Itbis18).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            b.Property(x => x.FechaEmision).HasColumnType("date");

            b.HasIndex(x => new { x.ContribuyenteId, x.NCF }).IsUnique();

            b.HasOne(x => x.Contribuyente)
                .WithMany(x => x.Comprobantes)
                .HasForeignKey(x => x.ContribuyenteId)
                .OnDelete(DeleteBehavior.Cascade);

      

            // Tipo NCF (opcional)
            b.HasOne(x => x.TipoNcf)
                .WithMany(x => x.Comprobantes)
                .HasForeignKey(x => x.TipoNcfId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}

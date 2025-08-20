using DGI_PruebaTecnica.Aplicacion.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Entities
{

    [Table("ComprobantesFiscales")]
    public sealed class ComprobanteFiscal : BaseEntity
    {
        [Column("ContribuyenteId")]
        public int ContribuyenteId { get; set; }

        [Column("NCF")]
        public string NCF { get; set; } = null!;

        [Column("Monto")]
        public decimal Monto { get; set; }

        [Column("TasaItbis")]
        public decimal TasaItbis { get; set; } = 18.00m;

        [Column("Itbis18")]
        public decimal Itbis18 { get; private set; } // Columna calculada en DB

        [Column("FechaEmision")]
        public DateTime? FechaEmision { get; set; }

        [Column("TipoNcfId")]
        public int? TipoNcfId { get; set; }

        // Nav
        public Contribuyente Contribuyente { get; set; } = null!;
        public TipoNCF? TipoNcf { get; set; }
    }
}

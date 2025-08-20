using DGI_PruebaTecnica.Aplicacion.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Entities
{
    [Table("Contribuyentes")]
    public sealed class Contribuyente : BaseEntity
    {
        [Column("RncCedula")]
        public string RncCedula { get; set; } = null!;

        [Column("Nombre")]
        public string Nombre { get; set; } = null!;

        [Column("TipoContribuyenteId")]
        public int TipoContribuyenteId { get; set; }

        [Column("EstatusContribuyenteId")]
        public int EstatusContribuyenteId { get; set; }

        [Column("FechaCreacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Nav
        public TipoContribuyente TipoContribuyente { get; set; } = null!;
        public EstatusContribuyente EstatusContribuyente { get; set; } = null!;
        public ICollection<ComprobanteFiscal> Comprobantes { get; set; } = new List<ComprobanteFiscal>();
    }
}

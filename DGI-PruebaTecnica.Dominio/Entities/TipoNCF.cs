using DGI_PruebaTecnica.Aplicacion.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Entities
{
    [Table("TipoNCF")]
    public sealed class TipoNCF : BaseEntity
    {
        [Column("Codigo")]
        public string Codigo { get; set; } = null!;

        [Column("Nombre")]
        public string Nombre { get; set; } = null!;

        // Nav
        public ICollection<ComprobanteFiscal> Comprobantes { get; set; } = new List<ComprobanteFiscal>();
    }
}

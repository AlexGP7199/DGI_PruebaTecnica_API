using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Dtos.Comprobantes.Response
{
    public sealed class ComprobanteListadoDto
    {
        public int Id { get; set; }
        public string RncCedula { get; set; } = null!;
        public string NCF { get; set; } = null!;
        public decimal Monto { get; set; }
        public decimal Itbis18 { get; set; }
        public DateTime? FechaEmision { get; set; }
    }
}

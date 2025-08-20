using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Dtos.Comprobantes.Response
{
    public sealed class ComprobanteResponse
    {
        public int id { get; set; }      // ← agregado
        public string rncCedula { get; set; } = null!;
        public string NCF { get; set; } = null!;
        public string monto { get; set; } = null!;   // "200.00"
        public string itbis18 { get; set; } = null!;   // "36.00"
        public string fechaEmision { get; set; } = null!;   // "2025-03-10" (ISO) o "" si null
    }
}

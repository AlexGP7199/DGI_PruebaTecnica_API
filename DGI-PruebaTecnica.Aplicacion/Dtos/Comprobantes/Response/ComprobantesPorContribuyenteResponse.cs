using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Dtos.Comprobantes.Response
{
    public sealed class ComprobantesPorContribuyenteResponse
    {
        public int contribuyenteId { get; set; } // ← agregado
        public string rncCedula { get; set; } = null!;
        public string nombre { get; set; } = null!;
        public string totalItbis { get; set; } = null!; // "216.00"
        public List<ComprobanteResponse> comprobantes { get; set; } = new();
    }
}

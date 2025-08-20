using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Dtos.Contribuyentes.Response
{
    public sealed class ContribuyenteResponse
    {
        public int id { get; set; }     
        public string rncCedula { get; set; } = null!;
        public string nombre { get; set; } = null!;
        public string tipo { get; set; } = null!;    // PERSONA FISICA | PERSONA JURIDICA
        public string estatus { get; set; } = null!;    // activo | inactivo
    }
}

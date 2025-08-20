using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Dtos.Contribuyentes.Response
{
    // PascalCase para que encaje con OrderingQuery
    public sealed class ContribuyenteListadoDto
    {
        public int Id { get; set; }
        public string RncCedula { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public string Estatus { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
    }
}

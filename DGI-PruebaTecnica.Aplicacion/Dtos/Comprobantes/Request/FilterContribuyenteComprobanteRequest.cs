using DGI_PruebaTecnica.Aplicacion.Commons.Bases.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Dtos.Comprobantes.Request
{
    public sealed class FilterContribuyenteComprobanteRequest : BaseFiltersRequest
    {
        /// <summary>Filtrar por RNC/Cédula del contribuyente (opcional).</summary>
        public string? RncCedula { get; set; }

      
    }
}

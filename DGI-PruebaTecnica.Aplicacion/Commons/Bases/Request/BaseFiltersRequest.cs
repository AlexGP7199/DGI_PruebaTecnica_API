using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Commons.Bases.Request
{
    public class BaseFiltersRequest : BasePaginationRequest
    {
        /// <summary>Selector de campo para búsquedas específicas (opcional).</summary>
        public int? NumFilter { get; set; } = null;

        /// <summary>Texto libre para búsqueda (RNC/NOMBRE/NCF, etc.).</summary>
        public string? TextFilter { get; set; } = null;

        /// <summary>Estado genérico (ej.: 1=Activo, 0=Inactivo) si aplica.</summary>
        public int? StateFilter { get; set; } = null;

        /// <summary>Rango de fechas (string para flexibilidad: "YYYY-MM-DD").</summary>
        public string? StartDate { get; set; } = null;
        public string? EndDate { get; set; } = null;

        /// <summary>Estatus textual si aplica (ej.: "activo" / "inactivo").</summary>
        public string? Estatus { get; set; } = null;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Commons.Bases.Request
{
    public class BasePaginationRequest
    {
        public int NumPage { get; set; } = 1;
        public int NumRecordPage { get; set; } = 10;

        public readonly int NumMaxRecordsPage = 50;

        /// <summary>asc | desc</summary>
        public string Order { get; set; } = "asc";

        /// <summary>Nombre de la columna para ordenar (ej.: "nombre", "rncCedula")</summary>
        public string? Sort { get; set; }

        public int Records
        {
            get => NumRecordPage;
            set => NumRecordPage = value > NumMaxRecordsPage ? NumMaxRecordsPage : value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Helpers.Interfaces
{
    public interface IDateRangeHelper
    {
        (DateTime? desde, DateTime? hasta) ParseDateRange(string? start, string? end);
    }
}

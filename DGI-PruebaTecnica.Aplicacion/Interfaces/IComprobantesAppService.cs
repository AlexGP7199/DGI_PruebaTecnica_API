using DGI_PruebaTecnica.Aplicacion.Commons.Bases.Response;
using DGI_PruebaTecnica.Aplicacion.Dtos.Comprobantes.Request;
using DGI_PruebaTecnica.Aplicacion.Dtos.Comprobantes.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Interfaces
{
    public interface IComprobantesAppService
    {
        Task<BaseResponse<List<ComprobanteResponse>>> ListarAsync(FilterContribuyenteComprobanteRequest request);
        Task<BaseResponse<ComprobantesPorContribuyenteResponse>> ObtenerPorContribuyenteAsync(ComprobantesPorContribuyenteRequest request);
    }
}

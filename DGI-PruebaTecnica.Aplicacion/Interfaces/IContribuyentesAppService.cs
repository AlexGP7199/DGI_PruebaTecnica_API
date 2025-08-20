using DGI_PruebaTecnica.Aplicacion.Commons.Bases.Response;
using DGI_PruebaTecnica.Aplicacion.Dtos.Contribuyentes.Request;
using DGI_PruebaTecnica.Aplicacion.Dtos.Contribuyentes.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Interfaces
{
    public interface IContribuyentesAppService
    {
        Task<BaseResponse<List<ContribuyenteResponse>>> ListarAsync(FilterContribuyenteRequest request);
        Task<BaseResponse<ContribuyenteResponse>> GetByIdAsync(int id);
    }
}

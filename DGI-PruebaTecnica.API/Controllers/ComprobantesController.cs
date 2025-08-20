using DGI_PruebaTecnica.Aplicacion.Dtos.Comprobantes.Request;
using DGI_PruebaTecnica.Aplicacion.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DGI_PruebaTecnica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprobantesController : ControllerBase
    {
        private readonly IComprobantesAppService _comprobantesAppService;

        public ComprobantesController(IComprobantesAppService comprobantesAppService)
        {
            _comprobantesAppService = comprobantesAppService;
        }

        /// <summary>
        /// Lista comprobantes con filtros, ordenamiento y paginación.
        /// </summary>
        /// <param name="filters">Parámetros de filtrado (RNC, texto, fechas, sort, etc.).</param>
        [HttpPost("listar")]
        public async Task<IActionResult> Listar([FromBody] FilterContribuyenteComprobanteRequest filters)
        {
            // Opcional: if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _comprobantesAppService.ListarAsync(filters);
            return Ok(response);
        }

        /// <summary>
        /// Obtiene los comprobantes de un contribuyente (con total de ITBIS) usando RNC/Cédula y filtros de fecha.
        /// </summary>
        /// <param name="request">RNC/Cédula, rango de fechas y filtros adicionales.</param>
        [HttpPost("contribuyente")]
        public async Task<IActionResult> ObtenerPorContribuyente([FromBody] ComprobantesPorContribuyenteRequest request)
        {
            // Opcional: if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _comprobantesAppService.ObtenerPorContribuyenteAsync(request);
            return Ok(response);
        }
    }
}

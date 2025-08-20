using DGI_PruebaTecnica.Aplicacion.Dtos.Contribuyentes.Request;
using DGI_PruebaTecnica.Aplicacion.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DGI_PruebaTecnica.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContribuyentesController : ControllerBase
    {
        private readonly IContribuyentesAppService _contribuyentesAppService;

        public ContribuyentesController(IContribuyentesAppService contribuyentesAppService)
        {
            _contribuyentesAppService = contribuyentesAppService;
        }

        /// <summary>
        /// Lista los contribuyentes con filtros, ordenamiento y paginación.
        /// </summary>
        /// <param name="filters">Parámetros de filtrado (texto, estado, fechas, etc.)</param>
        [HttpPost("listar")]
        public async Task<IActionResult> Listar([FromBody] FilterContribuyenteRequest filters)
        {
            var response = await _contribuyentesAppService.ListarAsync(filters);
            return Ok(response);
        }

        /// <summary>
        /// Obtiene un contribuyente por su ID.
        /// </summary>
        /// <param name="id">ID del contribuyente.</param>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _contribuyentesAppService.GetByIdAsync(id);
            return Ok(response);
        }
    }
}

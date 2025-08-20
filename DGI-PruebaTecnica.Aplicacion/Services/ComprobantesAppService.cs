using AutoMapper;
using DGI_PruebaTecnica.Aplicacion.Commons.Bases.Response;
using DGI_PruebaTecnica.Aplicacion.Commons.Ordering;
using DGI_PruebaTecnica.Aplicacion.Dtos.Comprobantes.Request;
using DGI_PruebaTecnica.Aplicacion.Dtos.Comprobantes.Response;
using DGI_PruebaTecnica.Aplicacion.Helpers;
using DGI_PruebaTecnica.Aplicacion.Helpers.Interfaces;
using DGI_PruebaTecnica.Aplicacion.Interfaces;
using DGI_PruebaTecnica.Infraestructura.Persistences.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Services
{
    public class ComprobantesAppService : IComprobantesAppService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IOrderingQuery _ordering;
        private readonly IDateRangeHelper _dateRange; // 👈 nuevo

        public ComprobantesAppService(IUnitOfWork uow, IMapper mapper, IOrderingQuery orderingQuery, IDateRangeHelper dateRange)
        {
            _uow = uow;
            _mapper = mapper;
            _ordering = orderingQuery;
            _dateRange = dateRange;
        }

        public async Task<BaseResponse<List<ComprobanteResponse>>> ListarAsync(FilterContribuyenteComprobanteRequest request)
        {
            var resp = new BaseResponse<List<ComprobanteResponse>>();

            try
            {
                // Base: solo activos (soft delete)
                var query = _uow.ComprobantesFiscales
                    .GetEntityQuery()
                    .Include(cf => cf.Contribuyente) // para RNC en el mapper
                    .AsNoTracking()
                    .AsQueryable();

                // 🔎 Filtros
                if (!string.IsNullOrWhiteSpace(request.RncCedula))
                {
                    var rnc = request.RncCedula.Trim();
                    query = query.Where(cf => cf.Contribuyente.RncCedula == rnc);
                }

                if (!string.IsNullOrWhiteSpace(request.TextFilter))
                {
                    var txt = request.TextFilter.Trim();
                    query = query.Where(cf =>
                        cf.NCF.Contains(txt) ||
                        cf.Contribuyente.RncCedula.Contains(txt));
                }

                // 🗓️ Rango de fechas (FechaEmision)
                var (desde, hasta) = _dateRange.ParseDateRange(request.StartDate, request.EndDate);
                if (desde.HasValue) query = query.Where(cf => cf.FechaEmision >= desde.Value);
                if (hasta.HasValue) query = query.Where(cf => cf.FechaEmision <= hasta.Value);

                // 🔢 Total antes de paginar
                var totalRecords = await query.CountAsync();

                // 🔃 Ordenar con tu OrderingQuery (sobre ENTIDAD)
                // ⚠️ Tu helper tiene fallback a "Nombre". Como ComprobanteFiscal no tiene "Nombre",
                // seteamos un sort por defecto seguro si viene vacío o inválido.
                if (string.IsNullOrWhiteSpace(request.Sort))
                    request.Sort = "FechaEmision"; // default seguro para esta entidad

                // TIP opcional: validar contra una whitelist para evitar fallos por props inexistentes
                var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { "Id", "NCF", "Monto", "Itbis18", "FechaEmision" };
                if (!allowed.Contains(request.Sort))
                    request.Sort = "FechaEmision";

                query = _ordering.Ordering(request, query); // NO uses pagination aquí

                // 📄 Paginación
                var data = await query
                    .Skip((request.NumPage - 1) * request.NumRecordPage)
                    .Take(request.NumRecordPage)
                    .ToListAsync();

                // 🧭 Map final
                var result = _mapper.Map<List<ComprobanteResponse>>(data);

                resp.IsSuccess = true;
                resp.Data = result;
                resp.TotalRecords = totalRecords;
                resp.Message = "Listado de comprobantes.";
                return resp;
            }
            catch (Exception ex)
            {
                resp.IsSuccess = false;
                resp.Message = $"Error al listar comprobantes: {ex.Message}";
                return resp;
            }
        }


        public async Task<BaseResponse<ComprobantesPorContribuyenteResponse>> ObtenerPorContribuyenteAsync(ComprobantesPorContribuyenteRequest request)
        {
            var resp = new BaseResponse<ComprobantesPorContribuyenteResponse>();

            try
            {
                if (string.IsNullOrWhiteSpace(request.RncCedula))
                {
                    resp.IsSuccess = false;
                    resp.Message = "Debe especificar el RNC/Cédula.";
                    return resp;
                }

                var rnc = request.RncCedula.Trim();

                // 1) Contribuyente (solo activos por soft delete)
                var qContrib = _uow.Contribuyentes
                    .GetEntityQuery(); // aplica x => x.Activo

                // Opcional: si usas StateFilter como EstatusContribuyenteId (catálogo)
                if (request.StateFilter.HasValue)
                {
                    var estatusId = request.StateFilter.Value;
                    qContrib = qContrib.Where(c => c.EstatusContribuyenteId == estatusId);
                }

                var contrib = await qContrib.FirstOrDefaultAsync(c => c.RncCedula == rnc);
                if (contrib is null)
                {
                    resp.IsSuccess = false;
                    resp.Message = "No se encontró el contribuyente.";
                    return resp;
                }

                // 2) Comprobantes del contribuyente (solo activos por soft delete)
                var comp = _uow.ComprobantesFiscales
                    .GetEntityQuery()
                    .Include(cf => cf.Contribuyente)   // para acceder al RNC en el DTO/mapper
                    .Where(cf => cf.ContribuyenteId == contrib.Id)
                    .AsNoTracking()
                    .AsQueryable();

                // 3) Filtro de fechas (FechaEmision)
                var (desde, hasta) = _dateRange.ParseDateRange(request.StartDate, request.EndDate);
                if (desde.HasValue) comp = comp.Where(cf => cf.FechaEmision >= desde.Value);
                if (hasta.HasValue) comp = comp.Where(cf => cf.FechaEmision <= hasta.Value);

                // 4) Total ITBIS
                var totalItbis = await comp.SumAsync(cf => (decimal?)cf.Itbis18) ?? 0m;

                // 5) Proyección a DTO intermedio (PascalCase) SIN joins manuales
                var detalleDto = comp.Select(cf => new ComprobanteListadoDto
                {
                    Id = cf.Id,
                    RncCedula = cf.Contribuyente.RncCedula,
                    NCF = cf.NCF,
                    Monto = cf.Monto,
                    Itbis18 = cf.Itbis18,
                    FechaEmision = cf.FechaEmision
                });

                // 6) Total de registros antes de paginar
                resp.TotalRecords = await detalleDto.CountAsync();

                // 7) Orden + paginación con tu helper
                // ⚠ Tu OrderingQuery tiene fallback "Nombre". Este DTO NO tiene Nombre.
                // Forzamos un sort por defecto seguro si viene vacío o inválido.
                if (string.IsNullOrWhiteSpace(request.Sort))
                    request.Sort = "Id";

                var allowedSort = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { "Id", "RncCedula", "NCF", "Monto", "Itbis18", "FechaEmision" };

                if (!allowedSort.Contains(request.Sort))
                    request.Sort = "Id";

                var pageDto = await _ordering
                    .Ordering(request, detalleDto, pagination: true)
                    .ToListAsync();

                // 8) Map final a response
                var items = _mapper.Map<List<ComprobanteResponse>>(pageDto);

                resp.Data = new ComprobantesPorContribuyenteResponse
                {
                    contribuyenteId = contrib.Id,
                    rncCedula = contrib.RncCedula,
                    nombre = contrib.Nombre,
                    totalItbis = totalItbis.ToString("F2", CultureInfo.InvariantCulture),
                    comprobantes = items
                };

                resp.IsSuccess = true;
                resp.Message = "Comprobantes por contribuyente obtenidos.";
                return resp;
            }
            catch (Exception ex)
            {
                resp.IsSuccess = false;
                resp.Message = $"Error al obtener comprobantes por contribuyente: {ex.Message}";
                return resp;
            }
        }



    }
}

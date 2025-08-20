using AutoMapper;
using DGI_PruebaTecnica.Aplicacion.Commons.Bases.Response;
using DGI_PruebaTecnica.Aplicacion.Commons.Ordering;
using DGI_PruebaTecnica.Aplicacion.Dtos.Contribuyentes.Request;
using DGI_PruebaTecnica.Aplicacion.Dtos.Contribuyentes.Response;
using DGI_PruebaTecnica.Aplicacion.Entities;
using DGI_PruebaTecnica.Aplicacion.Interfaces;
using DGI_PruebaTecnica.Infraestructura.Persistences.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace DGI_PruebaTecnica.Aplicacion.Services
{

    public class ContribuyentesAppService : IContribuyentesAppService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IOrderingQuery _ordering;

        public ContribuyentesAppService(IUnitOfWork uow, IMapper mapper, IOrderingQuery orderingQuery)
        {
            _uow = uow;
            _mapper = mapper;
            _ordering = orderingQuery;
        }
        public async Task<BaseResponse<List<ContribuyenteResponse>>> ListarAsync(FilterContribuyenteRequest request)
        {
            var resp = new BaseResponse<List<ContribuyenteResponse>>();

            try
            {
                // Base: solo activos (soft delete) vía GetEntityQuery()
                var query = _uow.Contribuyentes
                    .GetEntityQuery()
                    .Include(c => c.TipoContribuyente)
                    .Include(c => c.EstatusContribuyente)
                    .AsNoTracking()
                    .AsQueryable();

                // 🔎 Filtro de texto (NumFilter: 1=RNC/Cédula, 2=Nombre, default=ambos)
                if (!string.IsNullOrWhiteSpace(request.TextFilter))
                {
                    var txt = request.TextFilter.Trim().ToLower();

                    if (request.NumFilter == 1)
                        query = query.Where(c => c.RncCedula.ToLower().Contains(txt));
                    else if (request.NumFilter == 2)
                        query = query.Where(c => c.Nombre.ToLower().Contains(txt));
                    else
                        query = query.Where(c =>
                            c.RncCedula.ToLower().Contains(txt) ||
                            c.Nombre.ToLower().Contains(txt));
                }

                // 🎯 StateFilter = EstatusContribuyenteId (catálogo)
                if (request.StateFilter.HasValue)
                {
                    var estatusId = request.StateFilter.Value;
                    query = query.Where(c => c.EstatusContribuyenteId == estatusId);
                }

                // 🔤 Filtro por nombre de estatus (opcional)
                if (!string.IsNullOrWhiteSpace(request.Estatus))
                {
                    var estatusTxt = request.Estatus.Trim().ToLower();
                    query = query.Where(c =>
                        c.EstatusContribuyente != null &&
                        c.EstatusContribuyente.Nombre.ToLower().Contains(estatusTxt));
                }

                // 🗓️ Rango de fechas (FechaCreacion)
                if (!string.IsNullOrWhiteSpace(request.StartDate) &&
                    DateTime.TryParse(request.StartDate, out var start))
                {
                    query = query.Where(c => c.FechaCreacion >= start);
                }

                if (!string.IsNullOrWhiteSpace(request.EndDate) &&
                    DateTime.TryParse(request.EndDate, out var end))
                {
                    var endExclusive = end.Date.AddDays(1);
                    query = query.Where(c => c.FechaCreacion < endExclusive);
                }

                // 🔢 Total antes de paginar
                var totalRecords = await query.CountAsync();

                // 🔃 Ordenar con TU helper (sobre la entidad, igual que Amonestaciones)
                // Nota: tu helper espera propiedades del TDTO por reflexión.
                // En Contribuyente existen: Id, RncCedula, Nombre, FechaCreacion, etc.
                query = _ordering.Ordering(request, query);

                // 📄 Paginación
                var data = await query
                    .Skip((request.NumPage - 1) * request.NumRecordPage)
                    .Take(request.NumRecordPage)
                    .ToListAsync();

                // 🧭 Map al response
                var result = _mapper.Map<List<ContribuyenteResponse>>(data);

                resp.IsSuccess = true;
                resp.Data = result;
                resp.TotalRecords = totalRecords;
                resp.Message = "Listado de contribuyentes.";
                return resp;
            }
            catch (Exception ex)
            {
                resp.IsSuccess = false;
                resp.Message = $"Error al listar contribuyentes: {ex.Message}";
                return resp;
            }
        }


        public async Task<BaseResponse<ContribuyenteResponse>> GetByIdAsync(int id)
        {
            var entity = await _uow.Contribuyentes
                .GetEntityQuery(c => c.Id == id)
                .Include(c => c.TipoContribuyente)
                .Include(c => c.EstatusContribuyente)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (entity is null)
            {
                return new BaseResponse<ContribuyenteResponse>
                {
                    IsSuccess = false,
                    Message = "Contribuyente no encontrado."
                };
            }

            var dto = _mapper.Map<ContribuyenteResponse>(entity);

            return new BaseResponse<ContribuyenteResponse>
            {
                IsSuccess = true,
                Data = dto
            };
        }

    }
}
using AutoMapper;
using DGI_PruebaTecnica.Aplicacion.Dtos.Comprobantes.Response;
using DGI_PruebaTecnica.Aplicacion.Dtos.Contribuyentes.Response;
using DGI_PruebaTecnica.Aplicacion.Entities;


namespace DGI_PruebaTecnica.Aplicacion.MapperProfiles.Contribuyentes
{
    public sealed class ContribuyenteProfile : Profile
    {
        public ContribuyenteProfile()
        {
            // Entidad -> Response (incluyendo campos de catálogos por navegación)
            CreateMap<Contribuyente, ContribuyenteResponse>()
                .ForMember(d => d.id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.rncCedula, o => o.MapFrom(s => s.RncCedula))
                .ForMember(d => d.nombre, o => o.MapFrom(s => s.Nombre))
                .ForMember(d => d.tipo, o => o.MapFrom(s => s.TipoContribuyente != null ? s.TipoContribuyente.Nombre : string.Empty))
                .ForMember(d => d.estatus, o => o.MapFrom(s => s.EstatusContribuyente != null ? s.EstatusContribuyente.Nombre : string.Empty));

            // Solo cabecera: id/rnc/nombre; el total y la lista se llenan en el servicio
            CreateMap<Contribuyente, ComprobantesPorContribuyenteResponse>()
                .ForMember(d => d.contribuyenteId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.rncCedula, o => o.MapFrom(s => s.RncCedula))
                .ForMember(d => d.nombre, o => o.MapFrom(s => s.Nombre))
                .ForMember(d => d.totalItbis, o => o.Ignore())
                .ForMember(d => d.comprobantes, o => o.Ignore());

            // Del DTO intermedio (PascalCase) al response (camelCase)
            CreateMap<ContribuyenteListadoDto, ContribuyenteResponse>()
                .ForMember(d => d.id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.rncCedula, o => o.MapFrom(s => s.RncCedula))
                .ForMember(d => d.nombre, o => o.MapFrom(s => s.Nombre))
                .ForMember(d => d.tipo, o => o.MapFrom(s => s.Tipo))
                .ForMember(d => d.estatus, o => o.MapFrom(s => s.Estatus));

        }
    }
}

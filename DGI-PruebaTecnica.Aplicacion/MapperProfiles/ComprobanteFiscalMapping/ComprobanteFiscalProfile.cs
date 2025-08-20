using AutoMapper;
using DGI_PruebaTecnica.Aplicacion.Dtos.Comprobantes.Response;
using DGI_PruebaTecnica.Aplicacion.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.MapperProfiles.ComprobanteFiscalMapping
{
    public sealed class ComprobanteFiscalProfile : Profile
    {
        public ComprobanteFiscalProfile()
        {
            // Entidad -> Response (formateando decimales como string con 2 decimales)
            CreateMap<ComprobanteFiscal, ComprobanteResponse>()
                .ForMember(d => d.id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.rncCedula, o => o.MapFrom(s => s.Contribuyente != null ? s.Contribuyente.RncCedula : string.Empty))
                .ForMember(d => d.NCF, o => o.MapFrom(s => s.NCF))
                .ForMember(d => d.monto, o => o.MapFrom(s => s.Monto.ToString("F2", CultureInfo.InvariantCulture)))
                .ForMember(d => d.itbis18, o => o.MapFrom(s => s.Itbis18.ToString("F2", CultureInfo.InvariantCulture)))
                 .ForMember(d => d.fechaEmision, o => o.MapFrom(s =>
                    s.FechaEmision.HasValue
                        ? s.FechaEmision.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                        : string.Empty
                ));

            CreateMap<ComprobanteListadoDto, ComprobanteResponse>()
                .ForMember(d => d.id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.rncCedula, o => o.MapFrom(s => s.RncCedula))
                .ForMember(d => d.NCF, o => o.MapFrom(s => s.NCF))
                .ForMember(d => d.monto, o => o.MapFrom(s => s.Monto.ToString("F2", CultureInfo.InvariantCulture)))
                .ForMember(d => d.itbis18, o => o.MapFrom(s => s.Itbis18.ToString("F2", CultureInfo.InvariantCulture)))
                .ForMember(d => d.fechaEmision, o => o.MapFrom(s =>
                    s.FechaEmision.HasValue
                        ? s.FechaEmision.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                        : string.Empty));
        }
    }
}

using DGI_PruebaTecnica.Aplicacion.Commons.Ordering;
using DGI_PruebaTecnica.Aplicacion.Helpers.Interfaces;
using DGI_PruebaTecnica.Aplicacion.Helpers;
using DGI_PruebaTecnica.Aplicacion.Interfaces;
using DGI_PruebaTecnica.Aplicacion.MapperProfiles.ComprobanteFiscalMapping;
using DGI_PruebaTecnica.Aplicacion.MapperProfiles.Contribuyentes;
using DGI_PruebaTecnica.Aplicacion.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Extensions
{
    public static class InjectionExtions
    {
        public static IServiceCollection AddInjectionApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(configuration);

            // AutoMapper: registra todos los perfiles que viven en el assembly de cualquiera de estos tipos
            services.AddAutoMapper(cfg => { },
                typeof(ContribuyenteProfile).Assembly,
                typeof(ComprobanteFiscalProfile).Assembly);


            // Ordering (usa System.Linq.Dynamic.Core)
            services.AddScoped<IOrderingQuery, OrderingQuery>();

            // Helpers
            services.AddSingleton<IDateRangeHelper, DateRangeHelper>();

            // Application Services
            services.AddScoped<IContribuyentesAppService, ContribuyentesAppService>();
            services.AddScoped<IComprobantesAppService, ComprobantesAppService> ();

            return services;
        }
    }
}

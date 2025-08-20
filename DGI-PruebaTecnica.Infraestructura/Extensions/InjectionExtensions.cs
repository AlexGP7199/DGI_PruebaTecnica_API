using DGI_PruebaTecnica.Infraestructura.Persistences.Context;
using DGI_PruebaTecnica.Infraestructura.Persistences.Interfaces;
using DGI_PruebaTecnica.Infraestructura.Persistences.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Infraestructura.Extensions
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjectionInfrastructure(
              this IServiceCollection services,
              IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString("PruebaTecnicaConnection")
                ?? Environment.GetEnvironmentVariable("PRUEBA_TECNICA_DB_CONNECTION");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InvalidOperationException(
                    "No se encontró la cadena de conexión 'PruebaTecnicaConnection' ni la variable de entorno 'PRUEBA_TECNICA_DB_CONNECTION'.");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sql =>
                {
                    // Aunque no uses migraciones, no molesta dejar el assembly
                    sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    sql.CommandTimeout(60);
                });
           
            }, contextLifetime: ServiceLifetime.Scoped);

            // Repos genéricos y UoW
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}

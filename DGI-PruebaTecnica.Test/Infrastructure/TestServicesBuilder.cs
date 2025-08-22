using AutoMapper;
using DGI_PruebaTecnica.Aplicacion.Commons.Ordering;
using DGI_PruebaTecnica.Aplicacion.Entities;
using DGI_PruebaTecnica.Aplicacion.Helpers.Interfaces;
using DGI_PruebaTecnica.Aplicacion.Helpers;
using DGI_PruebaTecnica.Aplicacion.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DGI_PruebaTecnica.Infraestructura.Persistences.Interfaces;
using Microsoft.Extensions.Logging;

namespace DGI_PruebaTecnica.Test.Infrastructure
{
    public static class TestServicesBuilder
    {
        // helper para setear propiedad con private set usando EF
        private static void SetItbis(TestDbContext ctx, ComprobanteFiscal cf, decimal valor)
            => ctx.Entry(cf).Property(x => x.Itbis18).CurrentValue = valor;
        // ======== COMPROBANTES ========
        public static (TestDbContext ctx, IUnitOfWork uow, ComprobantesAppService sut) Build()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            var ctx = new TestDbContext(options);

            // Seed
            var c1 = new Contribuyente { Id = 1, RncCedula = "101010101", Nombre = "ACME SRL", EstatusContribuyenteId = 1, Activo = true };
            var c2 = new Contribuyente { Id = 2, RncCedula = "202020202", Nombre = "BETA SA", EstatusContribuyenteId = 2, Activo = true };
            ctx.Contribuyentes.AddRange(c1, c2);

            ctx.ComprobantesFiscales.AddRange(
                new ComprobanteFiscal { Id = 1, ContribuyenteId = 1, NCF = "A010", Monto = 1000m, FechaEmision = new DateTime(2024, 12, 31), Activo = true },
                new ComprobanteFiscal { Id = 2, ContribuyenteId = 1, NCF = "A020", Monto = 2000m, FechaEmision = new DateTime(2025, 01, 15), Activo = true },
                new ComprobanteFiscal { Id = 3, ContribuyenteId = 2, NCF = "B010", Monto = 500m, FechaEmision = new DateTime(2025, 02, 10), Activo = true }
            );

            // set Itbis18 via ChangeTracker (setter private)
            var f1 = ctx.ComprobantesFiscales.Local.First(x => x.Id == 1);
            var f2 = ctx.ComprobantesFiscales.Local.First(x => x.Id == 2);
            var f3 = ctx.ComprobantesFiscales.Local.First(x => x.Id == 3);

            SetItbis(ctx, f1, 180m);
            SetItbis(ctx, f2, 360m);
            SetItbis(ctx, f3, 90m);

            ctx.SaveChanges();

            var uow = new TestUnitOfWork(ctx);

            // AutoMapper con ILoggerFactory
            // Usa esta mínima si no instalaste providers:
            var loggerFactory = LoggerFactory.Create(_ => { });
            // Si instalaste Console/Debug: LoggerFactory.Create(b => b.AddDebug().AddConsole());

            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(ComprobantesAppService).Assembly);
            }, loggerFactory);

            var mapper = mapperCfg.CreateMapper();

            IOrderingQuery ordering = new OrderingQuery();
            IDateRangeHelper dateRange = new DateRangeHelper();

            var sut = new ComprobantesAppService(uow, mapper, ordering, dateRange);
            return (ctx, uow, sut);
        }

        // ======== CONTRIBUYENTES ========
        public static (TestDbContext ctx, IUnitOfWork uow, ContribuyentesAppService sut) BuildContribuyentes()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            var ctx = new TestDbContext(options);

            // Catálogos
            ctx.TipoContribuyentes.AddRange(
                new TipoContribuyente { Id = 1, Nombre = "Persona Física", Activo = true },
                new TipoContribuyente { Id = 2, Nombre = "Persona Jurídica", Activo = true }
            );
            ctx.EstatusContribuyentes.AddRange(
                new EstatusContribuyente { Id = 1, Nombre = "Activo", Activo = true },
                new EstatusContribuyente { Id = 2, Nombre = "Suspendido", Activo = true }
            );
            //ctx.TiposNCF.Add(new TipoNCF { Id = 1, Nombre = "Factura", Activo = true }); // por si el mapper usa esto en responses

            // Contribuyentes (asumiendo que BaseEntity tiene FechaCreacion; si no, elimina esa asignación)
            ctx.Contribuyentes.AddRange(
                new Contribuyente
                {
                    Id = 1,
                    RncCedula = "101010101",
                    Nombre = "ACME SRL",
                    TipoContribuyenteId = 2,
                    EstatusContribuyenteId = 1,
                    FechaCreacion = new DateTime(2025, 01, 10),
                    Activo = true
                },
                new Contribuyente
                {
                    Id = 2,
                    RncCedula = "202020202",
                    Nombre = "Beta Solutions",
                    TipoContribuyenteId = 2,
                    EstatusContribuyenteId = 2,
                    FechaCreacion = new DateTime(2025, 02, 05),
                    Activo = true
                },
                new Contribuyente
                {
                    Id = 3,
                    RncCedula = "303030303",
                    Nombre = "Carlos Pérez",
                    TipoContribuyenteId = 1,
                    EstatusContribuyenteId = 1,
                    FechaCreacion = new DateTime(2024, 12, 20),
                    Activo = true
                }
            );

            ctx.SaveChanges();

            var uow = new TestUnitOfWork(ctx);

            // AutoMapper con ILoggerFactory
            var loggerFactory = LoggerFactory.Create(_ => { });
            var mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(typeof(ContribuyentesAppService).Assembly);
            }, loggerFactory);
            var mapper = mapperCfg.CreateMapper();

            IOrderingQuery ordering = new OrderingQuery();

            var sut = new ContribuyentesAppService(uow, mapper, ordering);
            return (ctx, uow, sut);
        }
    }

}
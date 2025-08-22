using DGI_PruebaTecnica.Aplicacion.Dtos.Contribuyentes.Request;
using DGI_PruebaTecnica.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Test
{
    [TestClass]
    public class ContribuyentesAppService_MSTest
    {
        [TestMethod]
        public async Task ListarAsync_FiltroPorRnc_NumFilter1()
        {
            var (_, _, sut) = TestServicesBuilder.BuildContribuyentes();

            var req = new FilterContribuyenteRequest
            {
                TextFilter = "1010",   // debe encontrar "101010101"
                NumFilter = 1,         // 1 = RNC/Cédula
                NumPage = 1,
                NumRecordPage = 10,
                Sort = "Id",
                Order = "ASC"
            };

            var resp = await sut.ListarAsync(req);

            Assert.IsTrue(resp.IsSuccess);
            Assert.AreEqual(1, resp.Data.Count);
            Assert.AreEqual("101010101", resp.Data[0].rncCedula);
        }

        [TestMethod]
        public async Task ListarAsync_FiltroPorNombre_NumFilter2()
        {
            var (_, _, sut) = TestServicesBuilder.BuildContribuyentes();

            var req = new FilterContribuyenteRequest
            {
                TextFilter = "beta",   // "Beta Solutions"
                NumFilter = 2,         // 2 = Nombre
                NumPage = 1,
                NumRecordPage = 10,
                Sort = "Nombre",
                Order = "ASC"
            };

            var resp = await sut.ListarAsync(req);

            Assert.IsTrue(resp.IsSuccess);
            Assert.AreEqual(1, resp.Data.Count);
            Assert.AreEqual("Beta Solutions", resp.Data[0].nombre);
        }

        [TestMethod]
        public async Task ListarAsync_FiltroMixto_SinNumFilter()
        {
            var (_, _, sut) = TestServicesBuilder.BuildContribuyentes();

            var req = new FilterContribuyenteRequest
            {
                TextFilter = "30",
                NumPage = 1,
                NumRecordPage = 10,
                Sort = "Id",
                Order = "ASC"
            };

            var resp = await sut.ListarAsync(req);

            Assert.IsTrue(resp.IsSuccess);
            Assert.AreEqual(1, resp.Data.Count);
            Assert.AreEqual("303030303", resp.Data[0].rncCedula); // <<<<
        }


        [TestMethod]
        public async Task ListarAsync_StateFilter_PorEstatusId()
        {
            var (_, _, sut) = TestServicesBuilder.BuildContribuyentes();

            var req = new FilterContribuyenteRequest
            {
                StateFilter = 1, // "Activo"
                NumPage = 1,
                NumRecordPage = 10,
                Sort = "Id",
                Order = "ASC"
            };

            var resp = await sut.ListarAsync(req);

            Assert.IsTrue(resp.IsSuccess);
            // Activos en el seed: Id=1 (ACME) e Id=3 (Carlos) → 2 registros
            Assert.AreEqual(2, resp.Data.Count);
        }

        [TestMethod]
        public async Task ListarAsync_FiltroPorNombreDeEstatus_Texto()
        {
            var (_, _, sut) = TestServicesBuilder.BuildContribuyentes();

            var req = new FilterContribuyenteRequest
            {
                Estatus = "suspend", // "Suspendido"
                NumPage = 1,
                NumRecordPage = 10,
                Sort = "Id",
                Order = "ASC"
            };

            var resp = await sut.ListarAsync(req);

            Assert.IsTrue(resp.IsSuccess);
            Assert.AreEqual(1, resp.Data.Count);
            Assert.AreEqual("Beta Solutions", resp.Data[0].nombre); // <<<<
        }


        [TestMethod]
        public async Task ListarAsync_RangoFechas_OrdenacionYPaginacion()
        {
            var (_, _, sut) = TestServicesBuilder.BuildContribuyentes();

            var req = new FilterContribuyenteRequest
            {
                StartDate = "2024-12-25",
                EndDate = "2025-02-28",
                NumPage = 1,
                NumRecordPage = 2,
                Sort = "FechaCreacion",
                Order = "ASC"
            };

            var resp = await sut.ListarAsync(req);

            // En el seed: 2024-12-20 (queda fuera), 2025-01-10 (ACME) y 2025-02-05 (Beta) → 2
            Assert.IsTrue(resp.IsSuccess);
            Assert.AreEqual(2, resp.Data.Count);
            Assert.AreEqual("ACME SRL", resp.Data[0].nombre);
            Assert.AreEqual("Beta Solutions", resp.Data[1].nombre);
        }

        [TestMethod]
        public async Task ListarAsync_Paginado_OrdenDescPorNombre()
        {
            var (_, _, sut) = TestServicesBuilder.BuildContribuyentes();

            var req = new FilterContribuyenteRequest
            {
                NumPage = 1,
                NumRecordPage = 1,
                Sort = "Nombre", // ordenas por entidad; OK
                Order = "desc"
            };

            var resp = await sut.ListarAsync(req);

            Assert.IsTrue(resp.IsSuccess);
            Assert.AreEqual(1, resp.Data.Count);
            Assert.AreEqual("Carlos Pérez", resp.Data[0].nombre); // <<<<
            Assert.IsTrue(resp.TotalRecords >= 3);
        }


        [TestMethod]
        public async Task GetByIdAsync_NoEncontrado()
        {
            var (_, _, sut) = TestServicesBuilder.BuildContribuyentes();

            var resp = await sut.GetByIdAsync(999);

            Assert.IsFalse(resp.IsSuccess);
            Assert.AreEqual("Contribuyente no encontrado.", resp.Message);
        }

        [TestMethod]
        public async Task GetByIdAsync_Encontrado_IncluyeNavegacion()
        {
            var (_, _, sut) = TestServicesBuilder.BuildContribuyentes();

            var resp = await sut.GetByIdAsync(1);

            Assert.IsTrue(resp.IsSuccess);
            Assert.IsNotNull(resp.Data);
            Assert.AreEqual("ACME SRL", resp.Data!.nombre);
            // Si tu mapper proyecta nombres de catálogos, aquí podrías asertarlos también
        }
    }
}

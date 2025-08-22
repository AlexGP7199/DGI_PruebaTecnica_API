using DGI_PruebaTecnica.Aplicacion.Dtos.Comprobantes.Request;
using DGI_PruebaTecnica.Test.Infrastructure;

namespace DGI_PruebaTecnica.Test
{

    [TestClass]
    public class ComprobantesAppService_MSTest
    {
        [TestMethod]
        public async Task ListarAsync_Filtra_Texto_RangoFechas_Ordena_Pagina()
        {
            var (_, _, sut) = TestServicesBuilder.Build();

            var req = new FilterContribuyenteComprobanteRequest
            {
                RncCedula = "101010101",
                TextFilter = "A0",
                StartDate = "2024-12-01",
                EndDate = "2025-12-31",
                NumPage = 1,
                NumRecordPage = 1,
                Sort = "FechaEmision",
                Order = "desc"
            };

            var resp = await sut.ListarAsync(req);

            Assert.IsTrue(resp.IsSuccess);
            Assert.AreEqual(2, resp.TotalRecords);
            Assert.AreEqual(1, resp.Data.Count);
            Assert.AreEqual("A020", resp.Data[0].NCF);
        }

        [TestMethod]
         public async Task ListarAsync_SinSort_UsaDefault_FechaEmision_ASC()
        {
            var (_, _, sut) = TestServicesBuilder.Build();

            var req = new FilterContribuyenteComprobanteRequest
            {
                RncCedula = "101010101",
                NumPage = 1,
                NumRecordPage = 10,
                Sort = "",
                Order = "ASC"
            };

            var resp = await sut.ListarAsync(req);

            Assert.IsTrue(resp.IsSuccess);
            Assert.AreEqual(2, resp.Data.Count);
            Assert.AreEqual("A010", resp.Data[0].NCF);
        }

        [TestMethod]
        public async Task ObtenerPorContribuyenteAsync_Valida_Rnc_Obligatorio()
        {
            var (_, _, sut) = TestServicesBuilder.Build();

            var req = new ComprobantesPorContribuyenteRequest { RncCedula = "   " };
            var resp = await sut.ObtenerPorContribuyenteAsync(req);

            Assert.IsFalse(resp.IsSuccess);
            Assert.AreEqual("Debe especificar el RNC/Cédula.", resp.Message);
        }

        [TestMethod]
        public async Task ObtenerPorContribuyenteAsync_Rnc_Inexistente()
        {
            var (_, _, sut) = TestServicesBuilder.Build();

            var req = new ComprobantesPorContribuyenteRequest { RncCedula = "999999999" };
            var resp = await sut.ObtenerPorContribuyenteAsync(req);

            Assert.IsFalse(resp.IsSuccess);
            Assert.AreEqual("No se encontró el contribuyente.", resp.Message);
        }

        [TestMethod]
        public async Task ObtenerPorContribuyenteAsync_Suma_Itbis_Paginado_Orden()
        {
            var (_, _, sut) = TestServicesBuilder.Build();

            var req = new ComprobantesPorContribuyenteRequest
            {
                RncCedula = "101010101",
                StartDate = "2024-01-01",
                EndDate = "2025-12-31",
                NumPage = 1,
                NumRecordPage = 1,
                Sort = "Id",
                Order = "ASC"
            };

            var resp = await sut.ObtenerPorContribuyenteAsync(req);

            Assert.IsTrue(resp.IsSuccess);
            Assert.IsNotNull(resp.Data);
            Assert.AreEqual("540.00", resp.Data.totalItbis);
            Assert.AreEqual(2, resp.TotalRecords);
            Assert.AreEqual(1, resp.Data.comprobantes.Count);
            Assert.AreEqual("A010", resp.Data.comprobantes[0].NCF);
        }
    }
}
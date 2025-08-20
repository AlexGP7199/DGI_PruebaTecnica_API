
using DGI_PruebaTecnica.Aplicacion.Helpers.Interfaces;
using System.Globalization;
namespace DGI_PruebaTecnica.Aplicacion.Helpers
{
    public sealed class DateRangeHelper : IDateRangeHelper
    {
        public (DateTime? desde, DateTime? hasta) ParseDateRange(string? start, string? end)
        {
            DateTime? TryParse(string? s)
            {
                if (string.IsNullOrWhiteSpace(s)) return null;
                var fmts = new[] { "yyyy-MM-dd", "yyyy/MM/dd", "dd/MM/yyyy", "MM/dd/yyyy" };
                if (DateTime.TryParseExact(s.Trim(), fmts, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                    return dt;
                if (DateTime.TryParse(s, out dt)) return dt;
                return null;
            }

            var desde = TryParse(start);
            var hasta = TryParse(end);
            if (hasta.HasValue) hasta = hasta.Value.Date.AddDays(1).AddTicks(-1);
            return (desde, hasta);
        }
    }
}

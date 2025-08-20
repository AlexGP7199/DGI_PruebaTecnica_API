using DGI_PruebaTecnica.Aplicacion.Commons.Bases.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using DGI_PruebaTecnica.Aplicacion.Commons.Pagination;
namespace DGI_PruebaTecnica.Aplicacion.Commons.Ordering
{
    public class OrderingQuery : IOrderingQuery
    {
        public IQueryable<TDTO> Ordering<TDTO>(BasePaginationRequest request, IQueryable<TDTO> queryable, bool pagination = false) where TDTO : class
        {
            // 🔹 Validar que la propiedad Sort tenga un valor válido
            string sortProperty = string.IsNullOrEmpty(request.Sort)
         ? "Nombre"
         : char.ToUpper(request.Sort[0]) + request.Sort.Substring(1);

            var props = typeof(TDTO).GetProperties().Select(p => p.Name);
            if (!props.Contains(sortProperty))
                sortProperty = "Nombre"; // valor por defecto si no existe

            try
            {
                var queryDto = request.Order == "desc"
                    ? queryable.OrderBy($"{sortProperty} descending")
                    : queryable.OrderBy($"{sortProperty} ascending");

                return pagination ? queryDto.Paginate(request) : queryDto;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error en la ordenación: {ex.Message}. La propiedad de ordenación '{sortProperty}' no es válida.");
            }
          
        }
    }
}

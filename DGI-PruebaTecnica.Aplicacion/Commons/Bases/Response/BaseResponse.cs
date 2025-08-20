using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Commons.Bases.Response
{
    public class BaseResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public int TotalRecords { get; set; }

        public string? Message { get; set; }
        public IEnumerable<ValidationFailure>? Errores { get; set; }
        public bool? CorreoEnviado { get; set; }
    }
}

using DGI_PruebaTecnica.Aplicacion.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGI_PruebaTecnica.Aplicacion.Entities
{
    [Table("EstatusContribuyente")]
    public sealed class EstatusContribuyente : BaseEntity
    {
        [Column("Nombre")]
        public string Nombre { get; set; } = null!;

        //[Column("Activo")]
        //public bool Activo { get; set; } = true;

        // Nav
        public ICollection<Contribuyente> Contribuyentes { get; set; } = new List<Contribuyente>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.Domain.SPL.Masters
{
    public class CatSidcoInformation
    {
        public decimal Id { get; set; }
        public decimal? AttributeId { get; set; }
        public string ClaveSpl { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}

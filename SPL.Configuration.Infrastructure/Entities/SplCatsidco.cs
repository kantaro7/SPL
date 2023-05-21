using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Configuration.Infrastructure.Entities
{
    public partial class SplCatsidco
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

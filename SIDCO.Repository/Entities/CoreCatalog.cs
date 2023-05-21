using System;
using System.Collections.Generic;

#nullable disable

namespace SIDCO.Infrastructure.Entities
{
    public partial class CoreCatalog
    {
        public decimal Id { get; set; }
        public string Description { get; set; }
        public decimal? AttributeId { get; set; }
        public decimal? ParentId { get; set; }
        public string AparentId { get; set; }
    }
}

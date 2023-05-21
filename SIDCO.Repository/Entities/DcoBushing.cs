using System;
using System.Collections.Generic;

#nullable disable

namespace SIDCO.Infrastructure.Entities
{
    public partial class DcoBushing
    {
        public decimal BushingId { get; set; }
        public decimal? OrderId { get; set; }
        public decimal? StandardId { get; set; }
        public decimal? Active { get; set; }
        public string CreationUser { get; set; }
        public DateTime? CreationDatetime { get; set; }
        public string ModificationUser { get; set; }
        public DateTime? ModificationDatetime { get; set; }
        public string StandardOther { get; set; }
    }
}

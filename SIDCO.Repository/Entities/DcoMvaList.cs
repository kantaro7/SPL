using System;
using System.Collections.Generic;

#nullable disable

namespace SIDCO.Infrastructure.Entities
{
    public partial class DcoMvaList
    {
        public decimal MvaId { get; set; }
        public decimal OrderId { get; set; }
        public decimal Active { get; set; }
        public decimal? CoolingTypeId { get; set; }
        public decimal? OverElevationId { get; set; }
        public decimal? Mvaf1 { get; set; }
        public decimal? Mvaf2 { get; set; }
        public decimal? Mvaf3 { get; set; }
        public decimal? Mvaf4 { get; set; }
        public string CoolingTypeOther { get; set; }
        public string OverElevationOther { get; set; }
        public string OrCoolingTypeId { get; set; }
        public string OrOverElevationId { get; set; }
        public string OrMvaf1 { get; set; }
        public string OrMvaf2 { get; set; }
        public string OrMvaf3 { get; set; }
        public string OrMvaf4 { get; set; }
        public decimal? HstrId { get; set; }
        public decimal? HstrOther { get; set; }
        public string OrHstrId { get; set; }
        public decimal? DevAwr { get; set; }
        public string OrDevAwr { get; set; }
    }
}

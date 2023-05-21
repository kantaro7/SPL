using System;
using System.Collections.Generic;

#nullable disable

namespace SIDCO.Infrastructure.Entities
{
    public partial class DcoOrder
    {
        public decimal OrderId { get; set; }
        public string OrderCode { get; set; }
        public string InitialConsecutive { get; set; }
        public string FinalConsecutive { get; set; }
        public decimal? Revision { get; set; }
        public decimal? Active { get; set; }
        public string Comments { get; set; }
        public decimal? StatusId { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public decimal? Origin { get; set; }
        public string CreationUser { get; set; }
        public string ReleaseUser { get; set; }
        public string RevisionUser { get; set; }
        public decimal? ReportDi { get; set; }
        public decimal? IdSpct { get; set; }
    }
}

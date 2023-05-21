using System;
using System.Collections.Generic;

#nullable disable

namespace SIDCO.Infrastructure.Entities
{
    public partial class DcoSubmittalsDoc
    {
        public decimal? SubmittalsDocsId { get; set; }
        public decimal? OrderId { get; set; }
        public decimal? Active { get; set; }
        public string PoNumber { get; set; }
        public string GeReqNumeric { get; set; }
        public string Substation { get; set; }
        public string TagNumeric { get; set; }
        public decimal? MomClientNumeric { get; set; }
        public decimal? PhotograpsNumeric { get; set; }
        public decimal? PhotograpsId { get; set; }
        public decimal? LanguageId { get; set; }
        public decimal? UnitsId { get; set; }
        public DateTime? CreationDatetime { get; set; }
        public string CreationUser { get; set; }
        public DateTime? ModificationDatetime { get; set; }
        public string ModificationUser { get; set; }
        public string UnitsOther { get; set; }
    }
}

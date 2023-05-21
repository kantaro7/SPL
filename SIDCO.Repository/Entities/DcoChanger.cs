using System;
using System.Collections.Generic;

#nullable disable

namespace SIDCO.Infrastructure.Entities
{
    public partial class DcoChanger
    {
        public decimal? ChangerId { get; set; }
        public decimal? OrderId { get; set; }
        public decimal? Active { get; set; }
        public decimal? ColumnTypeId { get; set; }
        public decimal? OperationId { get; set; }
        public decimal? BrandId { get; set; }
        public string BrandOther { get; set; }
        public decimal? LocationId { get; set; }
        public string LocationOther { get; set; }
        public decimal? TypeId { get; set; }
        public string TypeOther { get; set; }
        public decimal? ModelId { get; set; }
        public string ModelOther { get; set; }
        public decimal? DerivId { get; set; }
        public string DerivOther { get; set; }
        public decimal? Taps { get; set; }
        public decimal? Imax { get; set; }
        public decimal? TrafoSerieId { get; set; }
        public decimal? TrafoRelation { get; set; }
        public decimal? BilId { get; set; }
        public string BilOther { get; set; }
        public decimal? CapInt { get; set; }
        public decimal? CtrlAutId { get; set; }
        public string CtrAutOther { get; set; }
        public decimal? CtrlEmpId { get; set; }
        public string CtrlEmpOther { get; set; }
        public DateTime? CreationDatetime { get; set; }
        public DateTime? ModificationDatetime { get; set; }
        public string CreationUser { get; set; }
        public string ModificationUser { get; set; }
        public decimal? DerivId2 { get; set; }
        public string Deriv2Other { get; set; }
        public decimal? RcbnFcbn { get; set; }
        public decimal? RowIndex { get; set; }
        public decimal? Type { get; set; }
        public decimal? TypeEngineId { get; set; }
        public decimal? ContactFinishId { get; set; }
        public string ContactFinishOther { get; set; }
        public decimal? MechanismHeightId { get; set; }
        public string MechanismHeightOther { get; set; }
        public string MechanismHeightSup { get; set; }
        public string MechanismHeightInf { get; set; }
        public decimal? LineNeutral { get; set; }
        public string Changer { get; set; }
        public string Description { get; set; }
    }
}

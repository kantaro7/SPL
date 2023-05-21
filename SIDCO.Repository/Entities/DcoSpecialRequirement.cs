using System;
using System.Collections.Generic;

#nullable disable

namespace SIDCO.Infrastructure.Entities
{
    public partial class DcoSpecialRequirement
    {
        public decimal SpecialReqId { get; set; }
        public decimal OrderId { get; set; }
        public decimal Active { get; set; }
        public decimal? OverloadStdId { get; set; }
        public string OverloadStdOther { get; set; }
        public decimal? OverExcitationStdId { get; set; }
        public string OverExcitationStdOther { get; set; }
        public decimal? PolarityId { get; set; }
        public string PolarityOther { get; set; }
        public decimal? CcStdId { get; set; }
        public string CcStdOther { get; set; }
        public string Consultant { get; set; }
        public string Format { get; set; }
        public decimal? FreeBucklingId { get; set; }
        public string FreeBucklingOther { get; set; }
        public decimal? ParallelOperationId { get; set; }
        public string ParallelOperationOther { get; set; }
        public decimal? CrimperId { get; set; }
        public string CrimperOther { get; set; }
        public decimal? EarthquakeCertificationId { get; set; }
        public string EarthquakeCertificationOther { get; set; }
        public string TensionLimit { get; set; }
        public string MaxContinousOperation { get; set; }
        public string MaxTempContinousOperation { get; set; }
        public string Crc { get; set; }
        public decimal? LEmb { get; set; }
        public decimal? AEmb { get; set; }
        public decimal? HEmbsa { get; set; }
        public decimal? HEmbca { get; set; }
        public decimal? PEmbsa { get; set; }
        public decimal? PEmbca { get; set; }
        public decimal? LOpe { get; set; }
        public decimal? AOpe { get; set; }
        public decimal? HOpe { get; set; }
        public decimal? POpe { get; set; }
        public decimal? VOpe { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string ModificationUser { get; set; }
        public string MaxContinousTempTmo { get; set; }
        public string MaxContinousTempMva { get; set; }
        public decimal? CapExtraTc { get; set; }
        public string CapExtraTcOther { get; set; }
        public decimal? CapExtraBoq { get; set; }
        public string CapExtraBoqOther { get; set; }
        public decimal? CapExtraCamCCarga { get; set; }
        public string CapExtraCamCCargaOther { get; set; }
        public decimal? CapExtraCamSCarga { get; set; }
        public string CapExtraCamSCargaOther { get; set; }
        public decimal? CapExtraComUnit { get; set; }
        public string CapExtraComUnitOther { get; set; }
        public decimal? SafetyMarginId { get; set; }
        public string SafetyMarginOther { get; set; }
        public decimal? FluidConditionId { get; set; }
        public string FluidConditionOther { get; set; }
    }
}

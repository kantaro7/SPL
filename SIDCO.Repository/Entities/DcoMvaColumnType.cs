using System;
using System.Collections.Generic;

#nullable disable

namespace SIDCO.Infrastructure.Entities
{
    public partial class DcoMvaColumnType
    {
        public decimal ColumnTypeId { get; set; }
        public decimal? DeviceId { get; set; }
        public string ColumnTitle { get; set; }
        public decimal? OrderIndex { get; set; }
        public decimal? Active { get; set; }
        public decimal? SectionId { get; set; }
        public string ConnectionCondition { get; set; }
        public string ConnectionField { get; set; }
        public string NbaiField { get; set; }
        public string VoltageField { get; set; }
        public string MvaField { get; set; }
        public string DerivationField { get; set; }
        public string SerParField { get; set; }
        public string Voltage2Field { get; set; }
        public string Derivation2Field { get; set; }
        public string TapsField { get; set; }
        public string RcbnFcbnField { get; set; }
        public decimal Type { get; set; }
    }
}

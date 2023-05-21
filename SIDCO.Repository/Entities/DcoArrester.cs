using System;
using System.Collections.Generic;

#nullable disable

namespace SIDCO.Infrastructure.Entities
{
    public partial class DcoArrester
    {
        public decimal? ArresterId { get; set; }
        public decimal? OrderId { get; set; }
        public decimal? Active { get; set; }
        public decimal? QtyId { get; set; }
        public decimal? BrandId { get; set; }
        public string BrandOther { get; set; }
        public decimal? TypeId { get; set; }
        public string TypeOther { get; set; }
        public decimal? MaterialId { get; set; }
        public string MaterialOther { get; set; }
        public decimal? VoltageId { get; set; }
        public decimal? VoltageOther { get; set; }
        public decimal? McovId { get; set; }
        public decimal? McovOther { get; set; }
        public decimal? DistanceLeak { get; set; }
        public decimal? BaseIns { get; set; }
        public decimal? CountDown { get; set; }
        public decimal? SopId { get; set; }
        public decimal? SopType { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDatetime { get; set; }
        public string CreationUser { get; set; }
        public string ModificationUser { get; set; }
        public decimal? ColumnTypeId { get; set; }
        public decimal? VoltageId2 { get; set; }
        public decimal? VoltageOther2 { get; set; }
        public decimal? McovId2 { get; set; }
        public decimal? McovOther2 { get; set; }
        public decimal? DistanceLeak2 { get; set; }
        public decimal? LandNetId { get; set; }
        public decimal? LandNet2Id { get; set; }
    }
}

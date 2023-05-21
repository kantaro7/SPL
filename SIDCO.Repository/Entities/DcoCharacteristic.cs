using System;
using System.Collections.Generic;

#nullable disable

namespace SIDCO.Infrastructure.Entities
{
    public partial class DcoCharacteristic
    {
        public decimal CharacteristicsId { get; set; }
        public decimal OrderId { get; set; }
        public decimal Active { get; set; }
        public decimal? PhaseId { get; set; }
        public decimal? FrequencyId { get; set; }
        public decimal? AltitudeF1Id { get; set; }
        public decimal? AltitudeF2Id { get; set; }
        public string AltitudeF1Other { get; set; }
        public string AltitudeF2Other { get; set; }
        public decimal? HstrId { get; set; }
        public string HstrOther { get; set; }
        public string OilElevation { get; set; }
        public string NucleousElevation { get; set; }
        public string TankElevation { get; set; }
        public decimal? OilPreservationId { get; set; }
        public decimal? IsolatorIncluded { get; set; }
        public decimal? LiquidIsolatorId { get; set; }
        public string LiquidIsolatorOther { get; set; }
        public decimal? StandardIsolatorId { get; set; }
        public string StandardIsolatorOther { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public string CreationUser { get; set; }
        public string ModificationUser { get; set; }
        public string DevAwr { get; set; }
        public decimal? OilLocationId { get; set; }
        public decimal? OilDefinedbyId { get; set; }
    }
}

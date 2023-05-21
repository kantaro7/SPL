using System;
using System.Collections.Generic;

#nullable disable

namespace SIDCO.Infrastructure.Entities
{
    public partial class DcoLabTestMain
    {
        public decimal? LabTestMainId { get; set; }
        public decimal? OrderId { get; set; }
        public decimal? Active { get; set; }
        public string TextTestRoutine { get; set; }
        public string TextTestDielectric { get; set; }
        public string TextTestPrototype { get; set; }
        public decimal? StandardId { get; set; }
        public decimal? TrafoType { get; set; }
        public decimal? MaxVoltage { get; set; }
        public decimal? MaxBil { get; set; }
        public decimal? FlagDeriv { get; set; }
        public decimal? MaxMva { get; set; }
        public decimal? PhaseId { get; set; }
        public DateTime? CreationDatetime { get; set; }
        public DateTime? ModificationDatetime { get; set; }
        public string CreationUser { get; set; }
        public string ModificationUser { get; set; }
        public decimal? TextTestId { get; set; }
        public decimal? FlagChanger { get; set; }
        public decimal? SaveData { get; set; }
        public decimal? Bilinducido { get; set; }
    }
}

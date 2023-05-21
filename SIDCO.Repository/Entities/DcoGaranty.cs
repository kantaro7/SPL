using System;
using System.Collections.Generic;

#nullable disable

namespace SIDCO.Infrastructure.Entities
{
    public partial class DcoGaranty
    {
        public decimal GarantyId { get; set; }
        public decimal OrderId { get; set; }
        public decimal? Active { get; set; }
        public decimal? Iexc100 { get; set; }
        public decimal? Iexc110 { get; set; }
        public decimal? Kwfe100 { get; set; }
        public decimal? Kwfe110 { get; set; }
        public decimal? Kwcu { get; set; }
        public decimal? KwcuMva { get; set; }
        public decimal? KwcuKv { get; set; }
        public decimal? Kwaux1 { get; set; }
        public decimal? Kwaux2 { get; set; }
        public decimal? Kwaux3 { get; set; }
        public decimal? Kwaux4 { get; set; }
        public decimal? Kwtot100 { get; set; }
        public decimal? Kwtot110 { get; set; }
        public decimal? ZPositiveMva { get; set; }
        public decimal? ZPositiveHx { get; set; }
        public decimal? ZPositiveHy { get; set; }
        public decimal? ZPositiveXy { get; set; }
        public decimal? ReacCl { get; set; }
        public decimal? InducCl { get; set; }
        public decimal? ResOhm { get; set; }
        public decimal? ZZeroMva { get; set; }
        public decimal? ZZeroHx { get; set; }
        public decimal? ZZeroHy { get; set; }
        public decimal? ZZeroXy { get; set; }
        public decimal? RelXr { get; set; }
        public decimal? NoiseOa { get; set; }
        public decimal? NoiseFa1 { get; set; }
        public decimal? NoiseFa2 { get; set; }
        public decimal? PenaltyCurrency { get; set; }
        public decimal? PenaltyKwfe { get; set; }
        public decimal? PenaltyCu { get; set; }
        public decimal? PenaltyAux { get; set; }
        public decimal? PenaltyTot { get; set; }
        public decimal? PenaltyMvaCu { get; set; }
        public decimal? PenaltyMvaAux { get; set; }
        public decimal? VibrationHz { get; set; }
        public decimal? VibrationMic { get; set; }
        public decimal? PowerFactor { get; set; }
        public decimal? TolerancyKwfe { get; set; }
        public decimal? TolerancyKwCu { get; set; }
        public decimal? TolerancyKwtot { get; set; }
        public decimal? TolerancyZpositive { get; set; }
        public string PenaltyCurrencyOther { get; set; }
        public DateTime? CreationDatetime { get; set; }
        public string CreationUser { get; set; }
        public DateTime? ModificationDatetime { get; set; }
        public string ModificationUser { get; set; }
        public decimal? TolerancyReac { get; set; }
        public decimal? TolerancyKwAux { get; set; }
        public decimal? TolerancyZzero { get; set; }
        public decimal? TolerancyZpositive2 { get; set; }
        public decimal? TolerancyZzero2 { get; set; }
        public decimal? ZPositiveTerc { get; set; }
        public decimal? ZZeroTerc { get; set; }
        public decimal? SaveData { get; set; }
        public decimal? NoiseNemaOa { get; set; }
        public decimal? NoiseNemaFa1 { get; set; }
        public decimal? NoiseNemaFa2 { get; set; }
        public decimal? NoiseLowOa { get; set; }
        public decimal? NoiseLowFa1 { get; set; }
        public decimal? NoiseLowFa2 { get; set; }
        public string ResinEpoxy { get; set; }
        public string RestraintPadsInn { get; set; }
        public string PanelOut { get; set; }
        public string PadsOut { get; set; }
        public decimal? ZDblvoltageMva { get; set; }
        public decimal? ZDblvoltageHz { get; set; }
        public decimal? ZDblvoltageXz { get; set; }
        public decimal? ZDblvoltageYz { get; set; }
        public decimal? TolerancyZdblvoltage { get; set; }
        public decimal? TolerancyZdblvoltage2 { get; set; }
        public decimal? ZZeroDblvoltageMva { get; set; }
        public decimal? ZZeroDblvoltageHz { get; set; }
        public decimal? ZZeroDblvoltageXz { get; set; }
        public decimal? ZZeroDblvoltageYz { get; set; }
        public decimal? TolerancyZzerodblvoltage { get; set; }
        public decimal? TolerancyZzerodblvoltage2 { get; set; }
    }
}

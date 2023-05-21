namespace SPL.Domain.SPL.Reports.PCI
{
    public class PCISecondaryPositionTest
    {
        #region Properties

        public string TapPosition { get; set; }

        public string Position { get; set; }

        public decimal CurrentIrms { get; set; }

        public decimal Voltage { get; set; }

        public decimal VoltagekVrms { get; set; }

        public decimal Power { get; set; }

        public decimal PowerKW { get; set; }

        public decimal AverageResistance { get; set; }

        public decimal ResistanceTemperature { get; set; }

        public decimal Tension { get; set; }

        public decimal VnomPrimary { get; set; }

        public decimal VnomSecondary { get; set; }

        public decimal InomPrimary { get; set; }

        public decimal InomSecondary { get; set; }

        public decimal WcuCor { get; set; }

        public decimal PrimaryReductionCorrectionFactor { get; set; }

        public decimal SecondaryReductionCorrectionFactor { get; set; }

        public decimal I2RPrimary { get; set; }

        public decimal I2RSecondary { get; set; }

        public decimal I2RTotal { get; set; }

        public decimal ElevationCorrectionFactor { get; set; }

        public decimal I2RCorrectedTotal { get; set; }

        public decimal Wind { get; set; }

        public decimal WindCorr { get; set; }

        public decimal WCu { get; set; }

        public decimal PercentageR { get; set; }

        public decimal PercentageZ { get; set; }

        public decimal PercentageX { get; set; }

        public decimal Corregidas20KW { get; set; }

        public decimal LossesCorrected { get; set; }

        public decimal LossesTotal { get; set; }

        #endregion
    }
}
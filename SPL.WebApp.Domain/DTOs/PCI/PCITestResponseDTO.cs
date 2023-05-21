namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class PCITestResponseDTO
    {
        public List<ErrorColumnsDTO> Messages { get; set; }

        public List<RatingPCITestResultDTO> Results { get; set; } = new List<RatingPCITestResultDTO>();

        public decimal Kwtot100M { get; set; }

        public bool IsApproved { get; set; }
    }

    public class RatingPCITestResultDTO
    {
        #region Properties

        public decimal BaseRating { get; set; }

        public string UmBaseRating { get; set; }

        public decimal TemperatureElevation { get; set; }

        public string UmTemperatureElevation { get; set; }

        public decimal Frequency { get; set; }

        public string UmFrequency { get; set; }

        public decimal Temperature { get; set; }

        public string UmTemperature { get; set; }

        public string TapPosition { get; set; }

        public string Position { get; set; }

        public decimal AverageResistance { get; set; }

        public decimal ResistanceTemperature { get; set; }

        public decimal Tension { get; set; }

        public List<SecondaryPositionPCITestResultDTO> SecondaryPositions { get; set; } = new List<SecondaryPositionPCITestResultDTO>();

        #endregion
    }

    public class SecondaryPositionPCITestResultDTO
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
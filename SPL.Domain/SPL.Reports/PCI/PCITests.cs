namespace SPL.Domain.SPL.Reports.PCI
{
    using System.Collections.Generic;

    public class PCITestRating
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

        public List<PCISecondaryPositionTest> SecondaryPositions { get; set; } = new List<PCISecondaryPositionTest>();

        #endregion
    }
}

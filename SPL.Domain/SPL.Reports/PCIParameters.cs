namespace SPL.Domain.SPL.Reports
{
    public class PCIParameters
    {
        #region Properties

        public string  PrimaryWinding { get; set; }
                       
        public string  PrimaryPosition { get; set; }
                       
        public decimal PrimaryTemperature { get; set; }
                       
        public decimal PrimaryAverageResistance { get; set; }

        public string SecondaryWinding { get; set; }

        public string SecondaryPosition { get; set; }

        public decimal SecondaryCorrection20 { get; set; }

        public decimal SecondaryTemperature { get; set; }

        public decimal SecondaryAverageResistance { get; set; }

        #endregion
    }
}
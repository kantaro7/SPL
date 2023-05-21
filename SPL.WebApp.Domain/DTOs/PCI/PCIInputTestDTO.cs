namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;
    public class PCIInputTestDTO
    {
        public bool ReducedCapacityAtLowVoltage { get; set; }

        public bool Autotransformer { get; set; }

        public bool Monofasico { get; set; }

        public string WindingMaterial { get; set; }

        public decimal Kwcu { get; set; }

        public decimal KwcuMva { get; set; }

        public decimal Kwtot100 { get; set; }

        public decimal TolerancyKwtot { get; set; }

        public string NominalSecondaryPosition { get; set; }

        public List<PCIRating> Ratings { get; set; } = new List<PCIRating>();
    }

    public class PCIRating
    {
        public decimal BaseRating { get; set; }

        public decimal TemperatureElevation { get; set; }

        public decimal Frequency { get; set; }

        public decimal Temperature { get; set; }

        public string TapPosition { get; set; }

        public string Position { get; set; }

        public decimal AverageResistance { get; set; }

        public decimal ResistanceTemperature { get; set; }

        public decimal Tension { get; set; }

        public List<PCISecondaryPosition> SecondaryPositions { get; set; } = new List<PCISecondaryPosition>();
    }

    public class PCISecondaryPosition
    {
        public string TapPosition { get; set; }

        public string Position { get; set; }

        public decimal CurrentIrms { get; set; }

        public decimal VoltagekVrms { get; set; }

        public decimal PowerKW { get; set; }

        public decimal ResistanceTemperature { get; set; }

        public decimal AverageResistance { get; set; }

        public decimal Corregidas20KW { get; set; }

        public decimal Tension { get; set; }
    }
}

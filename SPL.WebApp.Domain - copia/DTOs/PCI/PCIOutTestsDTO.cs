namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;
    public class PCIOutTestsDTO
    {
        public decimal BaseRating { get; set; }
        public string UmbaseRating { get; set; }
        public string UmFrequency { get; set; }
        public decimal Frequency { get; set; }
        public string UmTemperature { get; set; }
        public decimal Temperature { get; set; }
        public decimal ValorSE { get; set; }
        public string TapPositionsSec { get; set; }
        public string TapPositionsPrim { get; set; }
        public string WindingMaterial { get; set; }
        public bool Monofasico { get; set; }
        public bool Autotransformer { get; set; }
        public bool CapacityLow { get; set; }
        public WarrantiesArtifactDTO WarrantiesArtifact { get; set; }
        public List<PCITestsDTO> PCITests { get; set; }
        public decimal FacCorPri { get; set; }
        public decimal FacCorSec { get; set; }
        public decimal FacCorSe { get; set; }
        public string PriNom { get; set; }
        public string SecondNom { get; set; }
    }
}

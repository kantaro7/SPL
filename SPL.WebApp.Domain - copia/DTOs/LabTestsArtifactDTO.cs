namespace SPL.WebApp.Domain.DTOs
{
    using System;

    public class LabTestsArtifactDTO
    {
        public string OrderCode { get; set; }
        public string TextTestRoutine { get; set; }
        public string TextTestDielectric { get; set; }
        public string TextTestPrototype { get; set; }
        public string CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}

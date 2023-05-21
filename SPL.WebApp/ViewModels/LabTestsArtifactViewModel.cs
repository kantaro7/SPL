namespace SPL.WebApp.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class LabTestsArtifactViewModel
    {
        [Required(ErrorMessage = "OrderCodeLabTables")]
        public string OrderCode { get; set; }

        [DisplayName("Pruebas de rutina según")]
        public string TextTestRoutine { get; set; }

        [DisplayName("Pruebas Dieléctricas según")]
        public string TextTestDielectric { get; set; }

        [DisplayName("Pruebas Prototipo y Especiales según")]
        public string TextTestPrototype { get; set; }

        public string CreadoPor { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string ModificadoPor { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}

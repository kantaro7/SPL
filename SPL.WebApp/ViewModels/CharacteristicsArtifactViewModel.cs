namespace SPL.WebApp.ViewModels
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class CharacteristicsArtifactViewModel
    {
        [Required(ErrorMessage = "OrderCodeCharacteristic")]
        public string OrderCode { get; set; }

        public string Secuencia { get; set; }

        [DisplayName("Tipo Enfriamiento")]
        public string CoolingType { get; set; }

        [DisplayName("S.E.(*C)")]
        public string OverElevation { get; set; }

        [DisplayName("HSTR (*C)")]
        public string Hstr { get; set; }

        [DisplayName("Dev AWR")]
        public string DevAwr { get; set; }

        [DisplayName("Alta Tensión")]
        public string Mvaf1 { get; set; }

        [DisplayName("Baja Tensión")]
        public string Mvaf2 { get; set; }

        [DisplayName("2da. Baja")]
        public string Mvaf3 { get; set; }

        [DisplayName("Terciario")]
        public string Mvaf4 { get; set; }

        public string CreadoPor { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string ModificadoPor { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}

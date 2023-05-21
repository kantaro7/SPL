namespace SPL.WebApp.ViewModels.ETD
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ETD;

    public class RetdViewModel
    {
        public RetdViewModel()
        {
            this.Nuevo = new ();
            this.StabilizationDatas = new();
        }

        [Required(ErrorMessage = "Requerido")]
        [DisplayName("No. Serie")]
        [RegularExpression(@"^((G[0-9]{4}-[0-9]{2})|(G[0-9]{4}))$", ErrorMessage = "El No. de Serie debe cumplir con el formato G####-##")]
        public string NoSerie { get; set; }
        public string Error { get; set; }
        public List<string> CoolingTypes { get; set; }
        public List<decimal> OverElevations { get; set; }
        public decimal Altitude1 { get; set; }
        public string Altitude2 { get; set; }
        public decimal AltitudeFactor { get; set; }
        public PositionsDTO Positions { get; set; }
        public List<StabilizationDataViewModel> StabilizationDatas { get; set; }
        public StabilizationDataViewModel Nuevo { get; set; }
    }
}

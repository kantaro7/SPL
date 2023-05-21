namespace SPL.WebApp.ViewModels.ETD
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using SPL.WebApp.Domain.DTOs.ETD;

    public class RfckViewModel
    {
        public string Error { get; set; }
        public List<CorrectionFactorKWTypeCoolingDTO> CorrectionFactors { get; set; }
    }
}

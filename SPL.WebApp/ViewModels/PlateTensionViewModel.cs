namespace SPL.WebApp.ViewModels
{
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;

    public class PlateTensionViewModel
    {
        public PlateTensionViewModel()
        {

            TensionAT = new List<PlateTensionDTO>();
            TensionBT = new List<PlateTensionDTO>();
            TensionTER = new List<PlateTensionDTO>();
            TableAT = new DataTable();
            TableBT = new DataTable();
            TableTER = new DataTable();
            PositionTapBaan = new PositionTensionPlateDTO();
            Positions = new PositionTensionPlateDTO();
        }

        [RegularExpression(@"^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ0-9-@]*$",
            ErrorMessage = "Character no permitido.")]
        [Required(ErrorMessage = "No. Serie es requerido")]
        [DisplayName("No. Serie")]
        public string NoSerie { get; set; }

        public bool PositionValidate { get; set; }
        public bool LoadNewTension { get; set; }

        public PositionTensionPlateDTO PositionTapBaan { get; set; }

        public PositionTensionPlateDTO Positions { get; set; }

        public CharacteristicsPlaneTensionDTO CharacteristicsPlaneTension { get; set; }

        public TapBaanDTO TapBaan { get; set; }

        public List<PlateTensionDTO> TensionAT { get; set; }
        public List<PlateTensionDTO> TensionBT { get; set; }
        public List<PlateTensionDTO> TensionTER { get; set; }

        public DataTable TableAT { get; set; }
        public DataTable TableBT { get; set; }
        public DataTable TableTER { get; set; }
    }
}

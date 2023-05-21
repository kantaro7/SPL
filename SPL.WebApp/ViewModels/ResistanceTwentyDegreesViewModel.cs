namespace SPL.WebApp.ViewModels
{
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;

    public class ResistanceTwentyDegreesViewModel
    {
        public ResistanceTwentyDegreesViewModel()
        {

            this.ResistanceAT = new List<ResistDesignDTO>();
            this.ResistanceBT = new List<ResistDesignDTO>();
            this.ResistanceTER = new List<ResistDesignDTO>();
            this.TableAT = new DataTable();
            this.TableBT = new DataTable();
            this.TableTER = new DataTable();
            this.PositionTapBaan = new PositionTensionPlateDTO();
            this.Positions = new PositionTensionPlateDTO();

            this.ResistDesignHX = new List<ResistDesignDTO>();
            this.ResistDesignHN = new List<ResistDesignDTO>();
            this.ResistDesignHH = new List<ResistDesignDTO>();
            this.ResistDesignXN = new List<ResistDesignDTO>();
            this.ResistDesignXX = new List<ResistDesignDTO>();
            this.ResistDesignYN = new List<ResistDesignDTO>();
            this.ResistDesignYY = new List<ResistDesignDTO>();

            this.ResistDesignLLAT = new List<ResistDesignDTO>();
            this.ResistDesignLLBT = new List<ResistDesignDTO>();
            this.ResistDesignLLTER = new List<ResistDesignDTO>();

            this.ResistDesignLNAT = new List<ResistDesignDTO>();
            this.ResistDesignLNBT = new List<ResistDesignDTO>();
            this.ResistDesignLNTER = new List<ResistDesignDTO>();

            this.HayDataNueva = false;
            this.RequestInicial = true;
            this.AceptaCargarLaDataNueva = false;
        }

        [RegularExpression(@"^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ0-9-@]*$",
            ErrorMessage = "Character no permitido.")]
        [Required(ErrorMessage = "No. Serie es requerido")]
        [DisplayName("No. Serie")]
        public string NoSerie { get; set; }

        [RegularExpression(@"^[0-9]*$",
            ErrorMessage = "Character no permitido.")]
        [Required(ErrorMessage = "requerido")]
        [DisplayName("Temperatura (°C)")]
        public decimal Temperature { get; set; }

        [Required(ErrorMessage = "requerido")]
        [DisplayName("Unidad de Medida")]
        public string UnitMeasuring { get; set; }

        [Required(ErrorMessage = "requerido")]
        [DisplayName("Conexión de Prueba")]
        public string TestConnection { get; set; }

        public bool PositionValidate { get; set; }

        public bool LoadNewTension { get; set; }

        public PositionTensionPlateDTO PositionTapBaan { get; set; }

        public PositionTensionPlateDTO Positions { get; set; }

        public CharacteristicsPlaneTensionDTO CharacteristicsPlaneTension { get; set; }

        public TapBaanDTO TapBaan { get; set; }

        public List<ResistDesignDTO> ResistanceAT { get; set; }
        public List<ResistDesignDTO> ResistanceBT { get; set; }
        public List<ResistDesignDTO> ResistanceTER { get; set; }

        public DataTable TableAT { get; set; }
        public DataTable TableBT { get; set; }
        public DataTable TableTER { get; set; }

        /*******CORECCIONES ******************/
        public bool HN { get; set; }
        public bool XN { get; set; }
        public bool YN { get; set; }

        public List<ResistDesignDTO> ResistDesignHX { get; set; }
        public List<ResistDesignDTO> ResistDesignHN { get; set; }
        public List<ResistDesignDTO> ResistDesignHH { get; set; }
        public List<ResistDesignDTO> ResistDesignXN { get; set; }
        public List<ResistDesignDTO> ResistDesignXX { get; set; }
        public List<ResistDesignDTO> ResistDesignYN { get; set; }
        public List<ResistDesignDTO> ResistDesignYY { get; set; }

        public List<ResistDesignDTO> ResistDesignLLAT { get; set; }
        public List<ResistDesignDTO> ResistDesignLLBT { get; set; }
        public List<ResistDesignDTO> ResistDesignLLTER { get; set; }


        public List<ResistDesignDTO> ResistDesignLNAT { get; set; }
        public List<ResistDesignDTO> ResistDesignLNBT { get; set; }
        public List<ResistDesignDTO> ResistDesignLNTER { get; set; }


        public PositionsDTO PositionsDTO { get; set; }

        public bool HayDataNueva { get; set; }
        public bool RequestInicial { get; set; }
        public bool AceptaCargarLaDataNueva { get; set; }
        public bool IsEqualATSidCo { get; set; }
        public bool IsEqualBTSidCo { get; set; }
        public bool IsEqualTerSidCo { get; set; }

        public string TipoAparato { get; set; }

        /***********************************/
    }
}

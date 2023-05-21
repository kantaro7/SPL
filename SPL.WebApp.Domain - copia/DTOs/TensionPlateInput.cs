namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;
    using System.Data;

    public class TensionPlateInput
    {
        public TensionPlateInput()
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

        public PositionTensionPlateDTO PositionTapBaan { get; set; }

        public PositionTensionPlateDTO Positions { get; set; }
        public bool LoadNewTension { get; set; }

        public List<PlateTensionDTO> TensionAT { get; set; }
        public List<PlateTensionDTO> TensionBT { get; set; }
        public List<PlateTensionDTO> TensionTER { get; set; }

        public DataTable TableAT { get; set; }
        public DataTable TableBT { get; set; }
        public DataTable TableTER { get; set; }
    }
}

using SPL.WebApp.Domain.DTOs.NRA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.WebApp.Domain.DTOs
{
    public class SettingsToDisplayNRAReportsDTO
    {
        public HeadboardReportsDTO HeadboardReport { get; set; }
        public long NextTestNumber { get; set; }
        public string TypeCooling { get; set; }
        public string Warranty { get; set; }
        public string FoodTitle { get; set; }
        public string FoodUM { get; set; }
        public string FoodValue { get; set; }


        public string RuleTitle { get; set; }

        public List<MatrixOneDTO> MatrixBaseDto { get; set; }
        public List<MatrixOneDTO> MatrixOneDto { get; set; }
        public List<MatrixTwoDTO> MatrixTwoDto { get; set; }
        public MatrixTwoPromDTO MatrixTwoPromAntDespDto { get; set; }
        public MatrixTwoPromDTO MatrixTwoPromCoolingTypeDto { get; set; }
        public List<MatrixThreeDTO> MatrixThreeDto { get; set; }
        public MatrixThreeDTO AmbProm { get; set; }
        public MatrixThreeDTO AmbTrans { get; set; }

        public List<MatrixThree1323HDTO> MatrixHeight13 { get; set; }
        public List<MatrixThree1323HDTO> MatrixHeight23 { get; set; }
        public List<MatrixThree1323HDTO> MatrixHeight12 { get; set; }


        public List<MatrixThreeDTO> matrixThreeAnt { get; set; }
        public List<MatrixThreeDTO> matrixThreeDes { get; set; }
        public List<MatrixThreeDTO> matrixThreeCoolingType { get; set; }
        public decimal Diferencia { get; set; }
        public decimal FactorCoreccion { get; set; }
        //***Los datos que se deben mostrar en mas de una seccion no van aqui. Se hace mas facil hacerlos a nivel de fornt
        public List<ConfigurationReportsDTO> ConfigurationReports { get; set; }
        public BaseTemplateDTO BaseTemplate { get; set; }
        public InformationArtifactDTO InfotmationArtifact { get; set; }

        public PositionsDTO Posiciones { get; set; }
    }
}

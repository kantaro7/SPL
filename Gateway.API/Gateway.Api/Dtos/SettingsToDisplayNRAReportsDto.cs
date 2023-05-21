namespace Gateway.Api.Dtos
{
    using System.Collections.Generic;

    using Gateway.Api.Dtos.ArtifactDesing;
    using Gateway.Api.Dtos.Nozzle;

    public class SettingsToDisplayNRAReportsDto
    {
        public HeadboardReportsDto HeadboardReport { get; set; }
        public long NextTestNumber { get; set; }
        public string TypeCooling { get; set; }
        public string Warranty { get; set; }
        public string FoodTitle { get; set; }
        public string FoodUM { get; set; }
        public string FoodValue { get; set; }


        public string RuleTitle { get; set; }

        public List<MatrixOneDto> MatrixBaseDto { get; set; }
        public List<MatrixOneDto> MatrixOneDto { get; set; }
        public List<MatrixTwoDto> MatrixTwoDto { get; set; }
        public MatrixTwoPromDto MatrixTwoPromAntDespDto { get; set; }
        public MatrixTwoPromDto MatrixTwoPromCoolingTypeDto { get; set; }
        public List<MatrixThreeDto> MatrixThreeDto { get; set; }
        public MatrixThreeDto AmbProm { get; set; }
        public MatrixThreeDto AmbTrans { get; set; }

        public List<MatrixThree1323HDto> MatrixHeight12 { get; set; }
        public List<MatrixThree1323HDto> MatrixHeight13 { get; set; }
        public List<MatrixThree1323HDto> MatrixHeight23 { get; set; }


        public List<MatrixThreeDto> matrixThreeAnt { get; set; }
        public List<MatrixThreeDto> matrixThreeDes { get; set; }
        public List<MatrixThreeDto> matrixThreeCoolingType { get; set; }
        public decimal Diferencia { get; set; }
        public decimal FactorCoreccion { get; set; }
        //***Los datos que se deben mostrar en mas de una seccion no van aqui. Se hace mas facil hacerlos a nivel de fornt
        public List<ConfigurationReportsDto> ConfigurationReports { get; set; }
        public BaseTemplateDto BaseTemplate { get; set; }
        public InformationArtifactDto InfotmationArtifact { get; set; }
    }
}

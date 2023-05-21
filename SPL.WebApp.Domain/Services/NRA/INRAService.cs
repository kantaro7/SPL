namespace SPL.WebApp.Domain.Services
{
    using System.Collections.Generic;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.NRA;
    using Telerik.Web.Spreadsheet;

    public interface INraService
    {
        void PrepareTemplateNRA (SettingsToDisplayNRAReportsDTO reportsDTO, ref string tituloAlimentacion,ref List<MatrixThreeDTO>  matrizUnion ,  ref bool activarSegundasValidaciones ,ref Workbook workbook,
            string ClavePrueba, int cantidadColumnas, int cantidadValidaDeMediciones,string altura, bool esDataExistente, string lenguaje, string alimentacion, string cantidadAlimentacion, string tipoEnfriamiento, ref string medidaCorriente);
        string ValidateTemplate(SettingsToDisplayNRAReportsDTO reportsDTO, Workbook workbook, bool esCargarData, int CantidadMediciones, string pruebas, int columnas, string altura, string norma, string enfriamiento,
            List<InformationLaboratoriesDTO> info, string laboratorio, List<MatrixThreeDTO>matrizAnt, List<MatrixThreeDTO> matrizDes, List<MatrixThree1323HDTO> matriz12, List<MatrixThree1323HDTO> matriz13, List<MatrixThree1323HDTO> matriz23, PositionsDTO posiciones ,bool tieneSegundaSeccion , List<MatrixThreeDTO> UnionMatrices ,ref NRATestsGeneralDTO salida);

    }
}

namespace SPL.WebApp.Domain.Services
{
    using System.Collections.Generic;

    using SPL.WebApp.Domain.DTOs;

    using Telerik.Web.Spreadsheet;

    public interface IPciService
    {
        void PrepareTemplate_PCI (SettingsToDisplayPCIReportsDTO reportsDTO, ref Workbook workbook, string ClavePrueba, int cantidadPosicionesPrimarias , int catidadPosicionesSecundarias, string PosicionesPrimarias, string PosicionesSecundarias);

        string ValidateTemplatePCI(SettingsToDisplayPCIReportsDTO reportsDTO, Workbook workbook, string clavePrueba, string claveIdioma, string capacidadPrueba, int cantidadPosicionesPrimarias, int cantidadPosicionesSecundarias);

        string GetDatePCI(SettingsToDisplayPCIReportsDTO reportsDTO, Workbook workbook);

        string Prepare_PCI_Test(SettingsToDisplayPCIReportsDTO reportsDTO, Workbook workbook, string claveIdioma, int cantidadPosicionesPrimarias, int cantidadPosicionesSecundarias, string capacidad, string posicionPrimaria, string[] RegistrosPosicionesPrimarias, string posicionSecundaria, string[] RegistrosPosicionesSecundarias, List<PlateTensionDTO> plateTension, IEnumerable<PCIParameters> parameters, ref PCIInputTestDTO testOut);

        void PrepareIndexOfPCI(PCITestResponseDTO result, SettingsToDisplayPCIReportsDTO reportsDTO, string claveIdioma, int cantidadPosicionesPrimarias, int cantidadPosicionesSecundarias, ref Workbook workbook);
    }
}

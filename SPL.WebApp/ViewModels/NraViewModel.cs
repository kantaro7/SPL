namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.NRA;
    using SPL.WebApp.Validations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class NraViewModel
    {
        public NraViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();
        public PositionsDTO Positions { get; set; }
        public List<CharacteristicsArtifactDTO> CharacteristicsArtifact { get; set; }
        [DisplayName("No. Serie")]
        public string NoSerie { get; set; }
        [DisplayName("No Prueba")]
        public string NoPrueba { get; set; }
        [DisplayName("Idioma")]
        [Required(ErrorMessage = "Requerido")]
        public string ClaveIdioma { get; set; }
        [DisplayName("Prueba")]
        [Required(ErrorMessage = "Requerido")]
        public string Pruebas { get; set; }

        [DisplayName("Comentarios")]
        [MaxLength(300, ErrorMessage = "Límite máximo de Caracteres.")]
        public string Comments { get; set; }
        public string Date { get; set; }
        public List<TreeViewItemDTO> TreeViewItem { get; set; }
        public decimal AcceptanceValueFP { get; set; }
        public decimal AcceptanceValueCap { get; set; }
        public decimal TempFP { get; set; }
        public decimal TempTandD { get; set; }
        public Workbook Workbook { get; set; }
        public Workbook OfficialWorkbook { get; set; }
        public bool IsReportAproved { get; set; }
        public string Base64PDF { get; set; }
        public string MedicionCorriente { get; set; }
        public SettingsToDisplayNRAReportsDTO SettingsNRA { get; set; }
        public string Error { get; set; }
        public List<ArchivosFrontViewModel> Archivos { get; set; }

        [DisplayName("Laboratorio")]
        [Required(ErrorMessage = "Requerido")]
        public string Laboratorio { get; set; }

        [DisplayName("Norma")]
        [Required(ErrorMessage = "Requerido")]
        public string Norma { get; set; }

        [DisplayName("Alimentacion")]
        [Required(ErrorMessage = "Requerido")]
        public string Alimentacion { get; set; }

        [DisplayName("Alimentacion")]
        [Required(ErrorMessage = "Requerido")]
        public string Enfriamiento { get; set; }

        [DisplayName("kV / Amps / kW")]
        [Required(ErrorMessage = "Requerido")]
        [RegularExpression(@"^[0-9]{1,6}$|^[0-9]{1,6}\.[0-9]{1,3}$", ErrorMessage = "La alimentación debe ser numérica mayor a cero considerando 6 enteros con 3 decimales")]
        public string ValorAlimentacion { get; set; }



        [DisplayName("Cantidad de mediciones")]
        [Required(ErrorMessage = "Requerido")]
        public string CantMediciones { get; set; }



        [DisplayName("Fecha")]
        public DateTime? Fecha { get; set; }

        public bool CheckBox { get; set; }

        public string SelectCargaInformacion { get; set; }

        public string Notas { get; set; }



        [Required(ErrorMessage = "Requerido")]
        public SelectList Normas = new SelectList("");
        [Required(ErrorMessage = "Requerido")]
        public SelectList Enfriamientos = new SelectList("");

        public string Altura { get; set; }

        public List<InformationOctavesDTO> PruebasAntes { get; set; }
        public List<InformationOctavesDTO> PruebasDespues { get; set; }
        public List<InformationOctavesDTO> PruebasEnfriamiento { get; set; }

        public List<MatrixThreeDTO> matrixThreeAnt { get; set; }
        public List<MatrixThreeDTO> matrixThreeDes { get; set; }
        public List<MatrixThreeDTO> UnionMatrices { get; set; }



        public List<MatrixThree1323HDTO> MatrixHeight13 { get; set; }
        public List<MatrixThree1323HDTO> MatrixHeight23 { get; set; }
        public List<MatrixThree1323HDTO> MatrixHeight12 { get; set; }

        public bool EsCargaData { get; set; }
        public int CantidadColumnas { get; set; }
        public int CantidadMaximadeMediciones { get; set; }
        public string Warranty { get; set; }
        public string Resultado { get; set; }

        public string AlimentacionRespaldo { get; set; }

        public NRATestsGeneralDTO NRAPruebas { get; set; }
        public ResultNRATestsDTO NRAResults { get; set; }
        public bool ActivarSegundoHeader { get; set; }

    }

}

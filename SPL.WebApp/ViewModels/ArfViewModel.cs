namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Validations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class ArfViewModel
    {
        public ArfViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();
        public PositionsDTO Positions { get; set; }
        public List<CharacteristicsArtifactDTO> CharacteristicsArtifact { get; set; }
        [DisplayName("No. Serie")]
        public string NoSerie { get; set; }
        [DisplayName("No Prueba")]
        public string NoPrueba { get; set; }
        public decimal CantMediciones { get; set; }
        [DisplayName("Idioma")]
        [Required(ErrorMessage = "Requerido")]
        public string ClaveIdioma { get; set; }
        [DisplayName("Prueba")]
        [Required(ErrorMessage = "Requerido")]
        public string Pruebas { get; set; }


        [DisplayName("Comentarios")]
        [MaxLength(300, ErrorMessage = "Límite máximo de Caracteres.")]
        public string Comments { get; set; }

        [DisplayName("Número de Ciclos")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El campo Número de ciclos debe ser numérico mayor a cero considerando 6 enteros y sin decimales")]
        [Required(ErrorMessage = "Requerido")]
        [Range(1, 999999.99, ErrorMessage = "El campo Número de ciclos debe ser numérico mayor a cero considerando 6 enteros y sin decimales")]
        public string NroCiclosText { get; set; }

        [DisplayName("Número de Ciclos")]
        [Required(ErrorMessage = "Requerido")]
        public string NroCiclosSelect { get; set; }

        [DisplayName("Equipo")]
        [Required(ErrorMessage = "Requerido")]
        public string Equipo { get; set; }

        [DisplayName("Terciario o 2da.")]
        [Required(ErrorMessage = "Requerido")]
        public string TerciarioOSegunda { get; set; }

        [DisplayName("Terciario Disponible")]
        [Required(ErrorMessage = "Requerido")]
        public string TerciarioDisponible { get; set; }


        [DisplayName("Equipo")]
        [Required(ErrorMessage = "Terciario o 2da.")]
        public string TerciarioSegunda { get; set; }

        /******************LABORATORIO************************************/
        [DisplayName("Nivel de Aceite (Laboratorio)")]
        [Required(ErrorMessage = "Requerido")]
        public string NivelAceiteLab { get; set; }

        [DisplayName("Boquillas (Laboratorio)")]
        [Required(ErrorMessage = "Requerido")]
        public string BoquillasLab { get; set; }


        [DisplayName("Nucleos (Laboratorio)")]
        [Required(ErrorMessage = "Requerido")]
        public string NucleosLab { get; set; }


        [DisplayName("Terciario (Laboratorio)")]
        [Required(ErrorMessage = "Requerido")]
        public string TerciarioLab { get; set; }

        /******************LABORATORIO************************************/

        /******************LABORATORIO************************************/
        [DisplayName("Nivel de Aceite (Embarque)")]
        [Required(ErrorMessage = "Requerido")]
        public string NivelAceiteEmp { get; set; }

        [DisplayName("Boquillas (Embarque)")]
        [Required(ErrorMessage = "Requerido")]
        public string BoquillasEmp { get; set; }


        [DisplayName("Nucleos (Embarque)")]
        [Required(ErrorMessage = "Requerido")]
        public string NucleosEmp { get; set; }


        [DisplayName("Terciario (Embarque)")]
        [Required(ErrorMessage = "Requerido")]
        public string TerciarioEmp { get; set; }

        /******************LABORATORIO************************************/

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
        public string FechaSec1 { get; set; }
        public string FechaSec2 { get; set; }
        public TDPTestsGeneralDTO TestTDP { get; set; }
        public string[] ReigstrosPosicionesPrimarias { get; set; }
        public string[] ReigstrosPosicionesSecundarias { get; set; }
        public string Error { get; set; }



        [DisplayName("Total")]
        [Required(ErrorMessage = "Requerido")]
        [Range(1, 9999, ErrorMessage = "El tiempo total es numérico mayor a cero considerando 4 enteros sin decimales")]
        [RegularExpression(@"^[\d]*$", ErrorMessage = "El tiempo total es numérico mayor a cero considerando 4 enteros sin decimales")]
        public string TiempoTotal { get; set; }

        [DisplayName("Intervalo")]
        [Required(ErrorMessage = "Requerido")]
        [Range(1, 99, ErrorMessage = "El intervalo del tiempo es numérico mayor a cero considerando 2 enteros sin decimales")]
        [RegularExpression(@"^[\d]*$", ErrorMessage = "El intervalo del tiempo es numérico mayor a cero considerando 2 enteros sin decimales")]
        public string TiempoIntervalo { get; set; }


        [DisplayName("Nivel de la hora (kV)")]
        [Required(ErrorMessage = "Requerido")]
        [Range(1, 999, ErrorMessage = "El nivel de la hora es numérico mayor a cero considerando 3 enteros sin decimales")]
        [RegularExpression(@"^[\d]*$", ErrorMessage = "El nivel de la hora es numérico mayor a cero considerando 3 enteros sin decimales")]
        public string NivelHora { get; set; }

        [DisplayName("Nivel del Realce (kV)")]
        [Required(ErrorMessage = "Requerido")]
        [Range(1, 999, ErrorMessage = "El nivel del realce es numérico mayor a cero considerando 3 enteros sin decimales")]
        [RegularExpression(@"^[\d]*$", ErrorMessage = "El nivel del realce numérico mayor a cero considerando 3 enteros sin decimales")]
        public string NivelRealce { get; set; }

        [DisplayName("pC")]
        [Required(ErrorMessage = "Requerido")]
        [Range(1, 99999, ErrorMessage = "La descarga mayor para pC es numérico mayor a cero considerando 5 enteros sin decimales")]
        [RegularExpression(@"^[\d]*$", ErrorMessage = "La descarga mayor para pC es numérico mayor a cero considerando 5 enteros sin decimales")]
        public string DescargaPC { get; set; }

        [DisplayName("uV")]
        [Required(ErrorMessage = "Requerido")]
        [Range(1, 99999, ErrorMessage = "La descarga mayor para µV es numérico mayor a cero considerando 5 enteros sin decimales")]
        [RegularExpression(@"^[\d]*$", ErrorMessage = "La descarga mayor para µV es numérico mayor a cero considerando 5 enteros sin decimales")]
        public string DescargaUV { get; set; }

        [DisplayName("Incremento Máximo pC")]
        [Required(ErrorMessage = "Requerido")]
        [Range(1, 99999, ErrorMessage = "El incremento máximo pC es numérico mayor a cero considerando 5 enteros sin decimales")]
        [RegularExpression(@"^[\d]*$", ErrorMessage = "El incremento máximo pC es numérico mayor a cero considerando 5 enteros sin decimales")]
        public string IncrementoMaxPC { get; set; }

        [DisplayName("Niveles de Tensión")]
        [Required(ErrorMessage = "Requerido")]
        public string NivelesTension { get; set; }

        [DisplayName("Tipo de Medición")]
        [Required(ErrorMessage = "Requerido")]
        public string TipoMedicion { get; set; }

        [DisplayName("Terminales a Probar")]
        [Required(ErrorMessage = "Requerido")]
        public string TerminalesProbar { get; set; }

        public SelectList VoltageLevels { get; set; }
        public SelectList Terciarios { get; set; }

        public CeldasValidate CeldasPositions { get; set; }

        public SettingsToDisplayARFReportsDTO SettingsARF{ get; set; }

        public string Notes { get; set; }

        public string VoltageLevel { get; set; }

        public int Columnas { get; set; }


        public string AT1 { get; set; }
        public string AT2 { get; set; }
        public string BT2 { get; set; }
        public string BT1 { get; set; }
        public string Ter1 { get; set; }
        public string Ter2 { get; set; }
        public string Paginas { get; set; }
        public string Resultado { get; set; }
        public string TempAceite1 { get; set; }
        public string TempAceite2 { get; set; }

        public List<ArchivosFrontViewModel> Archivos { get; set; }


    }
}

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
    using System.IO;
    using Telerik.Web.Spreadsheet;

    public class ReportePruebasViewModel
    {
        public List<TreeViewItemDTO> TreeViewItem { get; set; }
        public ReportePruebasViewModel() => this.TreeViewItem = new List<TreeViewItemDTO>();
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

        public List<DataSourcePruebas> datasource { get; set; }

        public List<ReporteConsolidadoModel> ReportesConsolidados { get; set; }

        [DisplayName("Reporte")]
        [Required(ErrorMessage = "Requerido")]
        public string Reporte { get; set; }
    }

    public class DataSourcePruebas
    {
        public long ID_REP { get; set; }
        public string TIPO_REPORTE { get; set; }
        public string NOMBRE_REPORTE { get; set; }
        public string ID_PRUEBA { get; set; }
        public string PRUEBA { get; set; }
        public string FILTROS { get; set; }
        public DateTime? FECHA { get; set; }
        public string COMENTARIOS { get; set; }
        public List<DataSourcePruebas2> items { get; set; }
        public int rowLevel { get; set; }
        public bool expanded { get; set; }
        public bool isParent { get; set; }
        public bool isHeader { get; set; }
        public string isChecked { get; set; }
    }

    public class DataSourcePruebas2
    {
        public long ID_REP { get; set; }
        public string TIPO_REPORTE { get; set; }
        public string NOMBRE_REPORTE { get; set; }
        public string ID_PRUEBA { get; set; }
        public string PRUEBA { get; set; }
        public string FILTROS { get; set; }
        public string FECHA { get; set; }
        public string COMENTARIOS { get; set; }
        public int rowLevel { get; set; }
        public bool expanded { get; set; }
        public bool isParent { get; set; }
        public bool isHeader { get; set; }
        public bool isDefaultChecked { get; set; }
        public string isChecked { get; set; }

        public string AGRUPACION { get; set; }

        public string AGRUPACION_EN { get; set; }

        public string DESCRIPCION_EN { get; set; }
        public string IDIOMA { get; set; }
        public string RESULTADO { get; set; }

    }

}

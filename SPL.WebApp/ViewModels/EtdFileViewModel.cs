namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Http;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;

    public class EtdFileViewModel
    {
        public EtdFileViewModel()
        {
        }


        [DisplayName("Idioma")]
        [Required(ErrorMessage = "Requerido")]
        public string ClaveIdioma { get; set; }


        [DisplayName("Idioma")]
        [Required(ErrorMessage = "Requerido")]
        public string ClaveIdioma2 { get; set; }


        [RegularExpression(@"^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ0-9-@]*$",
            ErrorMessage = "Character no permitido.")]
        [Required(ErrorMessage = "No. Serie es requerido")]
        [DisplayName("No. Serie")]
        public string NoSerie { get; set; }

        [RegularExpression(@"^[A-Za-zäÄëËïÏöÖüÜáéíóúáéíóúÁÉÍÓÚÂÊÎÔÛâêîôûàèìòùÀÈÌÒÙ0-9-@]*$",
         ErrorMessage = "Character no permitido.")]
        [Required(ErrorMessage = "No. Serie es requerido")]
        [DisplayName("No. Serie")]
        public string NoSerie2 { get; set; }

        public bool Check1 { get; set; }
        public bool Check2 { get; set; }
        public bool Check3 { get; set; }

        public string Select1 { get; set; }
        public string Select2 { get; set; }
        public string Select3 { get; set; }

        [Required(ErrorMessage = "requerido")]
        [DisplayName("Tipo de Enfriamiento")]
        public string Enfriamiento { get; set; }
        public string OtroEnfriamiento { get; set; }

        [Required(ErrorMessage = "requerido")]
        [DisplayName("Tipo de Enfriamiento")]
        public string Enfriamiento2 { get; set; }

        [DisplayName("Otro")]
        public string Otro { get; set; }

        [DisplayName("Otro")]
        public string Otro2 { get; set; }

        public PositionsDTO PositionsDTO { get; set; }


        [DisplayName("Altitud Nro. 1")]
        public decimal? Altitud1 { get; set; }

        [DisplayName("Altitud Nro. 2")]
        public string Altitud2 { get; set; }

        [DisplayName("Cliente")]
        public string Cliente { get; set; }

        [DisplayName("Capacidades (Reporte)")]
        public string CapacidadReporte { get; set; }

        [DisplayName("Otro")]
        public string OtraCapacidad { get; set; }

        [Required(ErrorMessage = "requerido")]
        [DisplayName("Capacidad")]
        public string SelectCapacidades { get; set; }

        public List<decimal?>CapacidadesList { get; set; }
        public List<string> EnfriamientosList { get; set; }

        [DisplayName("Seleccione Archivo")]
        [Required(ErrorMessage = "Requerido")]
        public IFormFile File { get; set; }

        string ClavePrueba { get; set; }

    }
}

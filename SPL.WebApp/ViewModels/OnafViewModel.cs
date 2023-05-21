namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class OnafViewModel
    {
        [DisplayName("No. Serie")]
        public string NoSerie { get; set; }

        [DisplayName("No. Serie")]
        public string NoSerie2 { get; set; }

        [DisplayName("Tipo de Información")]
        [Required(ErrorMessage = "Requerido")]
        public string TipoInformacion { get; set; }
        
        [DisplayName("Plantilla Base")]
        public IFormFile File { get; set; }

        [DisplayName("Fecha Carga")]
        [Required(ErrorMessage = "Requerido")]
        public DateTime? Fecha { get; set; }

        [DisplayName("Altura")]
        [Required(ErrorMessage = "Requerido")]
        public string Altura { get; set; }

        public List<InformationOctavesDTO> Lista { get; set; }
        public List<string> Lista2 { get; set; }
        public SelectList TiposInformacion = new SelectList("");

        public bool AcceptaReemplazarData { get; set; }
        public string DataSource { get; set; }
        public List<string> ListaAlturas { get; set; }

        public bool IsFromFile { get; set; }   
    }
}
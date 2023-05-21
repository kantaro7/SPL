namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Mvc.Rendering;

    using SPL.WebApp.Domain.DTOs;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Telerik.Web.Spreadsheet;

    public class LimitCgdViewModel
    {
        public decimal Id { get; set; }

        [DisplayName("Reporte")]
        [Required(ErrorMessage = "Requerido")]
        public string TipoReporte { get; set; }

        [DisplayName("Prueba")]
        [Required(ErrorMessage = "Requerido")]
        public string ClavePrueba { get; set; }


        [DisplayName("Tipo de Aceite")]
        [Required(ErrorMessage = "Requerido")]
        public string TipoAceite { get; set; }

        [DisplayName("Limite Máximo")]
        [Required(ErrorMessage = "Requerido")]
        [Range(0.01, 999.99, ErrorMessage = "El limite máximo debe ser numérico mayor a cero considerando 3 enteros con 2 decimales")]
        [RegularExpression(@"^[0-9]{1,3}$|^[0-9]{1,3}\.[0-9]{1,2}$", ErrorMessage = "El limite máximo debe ser numérico mayor a cero considerando 3 enteros con 2 decimales")]

        public decimal LimiteMax { get; set; }

        public List<ContGasCGDDTO> CgdTestsDTOs { get; set; }

        public DateTime? Fechacreacion { get; set; }
        public DateTime? Fechamodificacion { get; set; }
        public string Creadopor { get; set; }
        public string Modificadopor { get; set; }
      
    }
}
